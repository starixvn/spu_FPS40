using Org.BouncyCastle.Crypto.Agreement;

namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Fanex.Oneyed.Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.ServiceProcess;

    public partial class Runner : ServiceBase
    {
        private static EasyNetQConnector _rabbitMQ;
        private ICapturingDataService _capturingDataService;
        private static List<RawTransactionModel> _transactionList;
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
        }

        protected override void OnStart(string[] args)
        {
            ConsumerTransactions();            
        }

        protected override void OnStop()
        {
            _rabbitMQ.Dispose();            
        }

        protected override void OnShutdown()
        {
            _rabbitMQ.Dispose();

            LogService.Info("Capturing data service has been shut down.");
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
            try
            {
                var trans = TransformRawTransaction(message);

                _transactionList.Add(trans);

                if (_transactionList.Count >= AppSettings.TransBatch)
                {
                    _capturingDataService.InsertTransaction(_transactionList);

                    _transactionList.Clear();

                    Console.WriteLine(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)                   
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }
        }

        private RawTransactionModel TransformRawTransaction(MessageQueue message)
        {
            var deviceCode = Utilities.GetDeviceCodeFromCapturedData(message.Device);
            var deviceInfo = _capturingDataService.GetDeviceInfo(deviceCode);

            return new RawTransactionModel()
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
    }
}