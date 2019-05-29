namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.Logging;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Fanex.Oneyed.Infrastructure.Models;
    using System;
    using System.ServiceProcess;

    public partial class Runner : ServiceBase
    {
        private static EasyNetQConnector _rabbitMQ;
        private ICapturingDataService _capturingDataService;

        private const string exchangeName = "fpstransExchangeTest";
        private const string queueName = "newfpstransQueueTest";
        private const string routingKey = "fpstransMessage";

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