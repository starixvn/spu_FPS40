namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using Fanex.NAP.FPS.Renovation.CapturingData.Service.Services;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using System.ServiceProcess;
    using System;
    using Fanex.Logging;

    public partial class Runner : ServiceBase
    {
        private static EasyNetQ _rabbitMQ;
        private ICapturingDataService _capturingDataService;
        private string _queuePrefix = "newfpstransQueue";
        private string _routingKey = "fpstransMessage";

        public Runner()
        {
            InitializeComponent();
            
            _rabbitMQ = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);
            
            //string strTimer = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + DateTime.Now.Millisecond.ToString("000");
            //string queueName = _queuePrefix + " " + strTimer + new Random().Next(0, 9000);

            //_rabbitMQ.CreateBus()
            //         .DeclareExchange(AppSetting.Exchange)
            //         .DeclareQueue(queueName)
            //         .DeclareRoutingKey(_routingKey)
            //         .Bind();

            _rabbitMQ.CreateBus()
                     .DeclareExchange(AppSetting.Exchange)
                     .DeclareQueue(_queuePrefix)
                     .DeclareRoutingKey(_routingKey)
                     .Bind();
        }

        protected override void OnStart(string[] args)
        {
            ConsumerCapturingTransaction();

            //LogService.Info("Capturing Data service has been started.");
        }

        protected override void OnStop()
        {
            _rabbitMQ.Dispose();

            //LogService.Info("Capturing Data service has been stopped.");
        }

        protected override void OnShutdown()
        {
            _rabbitMQ.Dispose();

            //LogService.Info("Capturing Data service has been shut down.");
        }      
        
        private void ConsumerCapturingTransaction()
        {
            _rabbitMQ.AdvanceBus.Consume(_rabbitMQ.Queue, x => x.Add<CapturingTransactionMessage>((message, info) =>
            {
                ProcessMessage(message.Body);
            }));
        }

        private void ProcessMessage(CapturingTransactionMessage message)
        {
            _capturingDataService = new CapturingDataService(message);
            _capturingDataService.InsertCapturingTransaction();
        }

    }
}