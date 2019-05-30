using Org.BouncyCastle.Crypto.Agreement;

namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.Logging;
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
        private static List<RawTransaction> _transactionList;
        public Runner()
        {
            InitializeComponent();

            _capturingDataService = new CapturingDataService();

            _rabbitMQ = new EasyNetQConnector(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);
            _rabbitMQ.CreateBus()
                     .DeclareExchange(AppSetting.Exchange)
                     .DeclareQueue(AppSetting.QueueName)
                     .DeclareRoutingKey(AppSetting.RoutingKey)
                     .Bind();

            _transactionList = new List<RawTransaction>();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ConsumerTransactions();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);


                throw;
            }


            LogService.Info("Capturing data service has been started.");
        }

        protected override void OnStop()
        {
            _rabbitMQ.Dispose();

            LogService.Info("Capturing data service has been stopped.");
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

                if (_transactionList.Count >= 10)
                {
                    _capturingDataService.InsertTransaction(_transactionList);

                    _transactionList.Clear();
                }
            }
            catch (Exception ex)
            {
                LogService.Error("Insert Capturing data fail: " + ex.ToString());
            }
        }

        private RawTransaction TransformRawTransaction(MessageQueue message)
        {
            var ipInfo = _capturingDataService.GetIpInfo(message.IP);
            var deviceCode = Utilities.GetDeviceCodeFromCapturedData(message.Device);
            var deviceInfo = _capturingDataService.GetDeviceInfo(deviceCode);

            return new RawTransaction()
            {
                Action = message.Action,
                IP = message.IP,
                Token = message.Token,
                CaptchaResult = String.Empty,
                SubscriberName = message.Subscriber,
                LoginName = message.Account,
                DeviceCode = deviceInfo.DeviceNo,
                ActionResult = message.ActionResult,
                TransDate = message.TransDate.ToString("yyyy-MM-dd hh:mm:ss"),
                PluginID = 0,
                DeviceStatus = 0,
                URL = deviceInfo.HRef,
                UserAgent = deviceInfo.UserAgent,
                CapturedData = message.Device,
                CountryCode = ipInfo != null && !string.IsNullOrEmpty(ipInfo.CountryCode) ? ipInfo.CountryCode : string.Empty,
                Region = ipInfo != null && !string.IsNullOrEmpty(ipInfo.Region) ? ipInfo.Region : string.Empty,
                City = ipInfo != null && !string.IsNullOrEmpty(ipInfo.City) ? ipInfo.City : string.Empty,
                ISP = ipInfo != null && !string.IsNullOrEmpty(ipInfo.ISP) ? ipInfo.ISP : string.Empty,
                Flag = ipInfo != null && !string.IsNullOrEmpty(ipInfo.Flag) ? ipInfo.Flag : string.Empty,
                Country = ipInfo != null && !string.IsNullOrEmpty(ipInfo.CountryName) ? ipInfo.CountryName : string.Empty
            };
        }
    }
}