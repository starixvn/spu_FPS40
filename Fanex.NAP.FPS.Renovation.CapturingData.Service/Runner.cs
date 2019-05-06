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

        private Thread _threadProducerTest = null;
        private Thread _threadConsumer = null;       

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
        }

        protected override void OnStart(string[] args)
        {
            #region test transaction producer
            //_threadProducerTest = new Thread(new ThreadStart(ThreadProducerTest));
            //_threadProducerTest.Start();
            #endregion test transaction producer

            _threadConsumer = new Thread(new ThreadStart(ConsumerCapturingTransaction));
            _threadConsumer.Start();
        }

        protected override void OnStop()
        {
            _rabbitMQ.Dispose();
        }

        protected override void OnShutdown()
        {
            _rabbitMQ.Dispose();
        }

        #region test transaction producer

        private void ThreadProducerTest()
        {
            TransactionProducerTest.PublishTransactionMessage();            
        }

        #endregion test transaction producer

        private void ConsumerCapturingTransaction()
        {
            int interval = 5000;
            int elapsed = 0;

            int waitTime = 1000;
            try
            {
                while (true)
                {
                    if (interval <= elapsed)
                    {
                        elapsed = 0;
                        GetMessage();
                    }
                    Thread.Sleep(waitTime);
                    elapsed += waitTime;
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        private void GetMessage()
        {
            _rabbitMQ.DeclareQueue("newfpstransQueue")
                       .DeclareRoutingKey("newfpstransQueueKey")
                       .Bind();

            CapturingTransactionMessage capturingTransactionMessage = null;

            _rabbitMQ.AdvanceBus.Consume(_rabbitMQ.Queue, x => x.Add<CapturingTransactionMessage>((message, info) =>
            {
                capturingTransactionMessage = message.Body;
                ProcessMessage(capturingTransactionMessage);
            }));
        }

        private void ProcessMessage(CapturingTransactionMessage message)
        {
            _capturingDataService = new CapturingDataService(message);
            _capturingDataService.InsertCapturingTransaction();
        }

    }
}
