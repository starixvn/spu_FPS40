namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Fanex.Oneyed.Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.ServiceProcess;
    using System.Timers;

    public partial class Runner : ServiceBase
    {
        private readonly EasyNetQConnector _rabbitMQ;
        private readonly ICapturingDataService _capturingDataService;
        private readonly List<RawTransactionModel> _transactionList;
        private readonly Timer _timer;
        private DateTime _lastInsertTrans;
        public Runner()
        {
            InitializeComponent();

            _capturingDataService = new CapturingDataService();

            _rabbitMQ = new EasyNetQConnector(AppSettings.Host + ":" + AppSettings.Post,
                                       AppSettings.VirtualHost,
                                       AppSettings.PrefetchCount,
                                       AppSettings.UserName,
                                       AppSettings.Password);
            _rabbitMQ.CreateBus()
                     .DeclareExchange(AppSettings.Exchange)
                     .DeclareQueue(AppSettings.QueueName)
                     .DeclareRoutingKey(AppSettings.RoutingKey)
                     .Bind();

            _transactionList = new List<RawTransactionModel>();

            _timer = new Timer();
            _timer.Elapsed += InsertTransactionByTimer;
            _timer.Interval = AppSettings.Interval;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ConsumerTransactions();

                if (!_timer.Enabled)
                {
                    _timer.Start();
                }

                LogService.Info($"Raw trans service is STARTED at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }
        }

        protected override void OnStop()
        {
            _rabbitMQ.Disposed();

            LogService.Info($"Raw trans service is STOPED at {DateTime.Now}");
        }

        protected override void OnShutdown()
        {
            _rabbitMQ.Disposed();

            LogService.Info($"Raw trans service is STOPED at {DateTime.Now}");
        }

        private void ConsumerTransactions()
        {
            _rabbitMQ.AdvanceBus.Consume(_rabbitMQ.Queue, x => x.Add<MessageQueue>((message, info) =>
            {
                ProcessMessage(message.Body);
            }));
        }

        private void ProcessMessage(MessageQueue message)
        {
            var trans = TransformRawTransaction(message);

            _transactionList.Add(trans);

            if (_transactionList.Count >= AppSettings.TransBatch)
            {
                InsertTransaction();
            }
        }

        private void InsertTransaction()
        {
            _capturingDataService.InsertTransaction(_transactionList);

            _lastInsertTrans = DateTime.Now;

            _transactionList.Clear();
        }

        private RawTransactionModel TransformRawTransaction(MessageQueue message)
        {
            var deviceCode = Utilities.GetDeviceCodeFromCapturedData(message.Device);
            var deviceInfo = _capturingDataService.GetDeviceInfo(deviceCode);

            return new RawTransactionModel
            {
                DeviceCode = deviceInfo.DeviceNo,
                LoginName = message.Account,
                SubscriberName = message.Subscriber,
                TransDate = Convert.ToDateTime(message.TransDate.ToString("yyyy-MM-dd hh:mm:ss")),
                IP = message.IP,
                PluginID = 0,
                DeviceStatus = 0,
                URL = deviceInfo.HRef,
                ActionResult = message.ActionResult,
                UserAgent = deviceInfo.UserAgent,
                CapturedData = message.Device,
            };
        }

        private void InsertTransactionByTimer(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - _lastInsertTrans).Minutes >= 1 && _transactionList.Count != 0)
            {
                InsertTransaction();
            }
        }
    }
}