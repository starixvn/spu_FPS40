namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using Fanex.NAP.FPS.Renovation.CapturingData.Service.Services;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Fanex.NAP.FPS.Renovation.Core;    
    using System.ServiceProcess;
    using System;
    using Fanex.Logging;
    using Fanex.Oneyed.Infrastructure.Models;

    public partial class Runner : ServiceBase
    {
        private static EasyNetQ _rabbitMQ;
        private ICapturingDataService _capturingDataService;

        private const string exchangeName = "fpstransExchangeTest";
        private const string queueName = "newfpstransQueue";
        private const string routingKey = "fpstransMessage";

        public Runner()
        {
            InitializeComponent();
            _capturingDataService = new CapturingDataService();

            _rabbitMQ = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);           
            _rabbitMQ.CreateBus()
                     .DeclareExchange(exchangeName)
                     .DeclareQueue(queueName)
                     .DeclareRoutingKey(routingKey)
                     .Bind();       
        }

        protected override void OnStart(string[] args)
        {
            ConsumerTransactions();
            
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
                _capturingDataService.InsertTransaction(message);
            }
            catch (Exception ex)
            {
                LogService.Error("Insert Capturing data fail: " + ex.ToString());
            }            
        }
      
    }
}