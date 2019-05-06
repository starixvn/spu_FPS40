namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using Fanex.NAP.FPS.Renovation.CapturingData.Service.Services;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using System.ServiceProcess;
    using System.Threading;      

    public partial class Runner : ServiceBase
    {
        private static EasyNetQ _rabbitMQ;
        private CapturingDataService _capturingDataService;

        public Runner()
        {
            InitializeComponent();
            
            _rabbitMQ = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);

            _rabbitMQ.CreateBus()
                     .DeclareExchange(AppSetting.Exchange);

            _rabbitMQ.DeclareQueue("newfpstransQueue")
                       .DeclareRoutingKey("newfpstransQueueKey")
                       .Bind();
        }

        protected override void OnStart(string[] args)
        {
            ConsumerCapturingTransaction();
        }

        protected override void OnStop()
        {
            _rabbitMQ.Dispose();
        }

        protected override void OnShutdown()
        {
            _rabbitMQ.Dispose();
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
