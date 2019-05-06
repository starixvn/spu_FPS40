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

            _capturingDataService = new CapturingDataService();
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
            #region Test on localhost
            // test transaction producer
            //_threadProducerTest = new Thread(new ThreadStart(ThreadProducerTest));
            //_threadProducerTest.Start();

            //_threadConsumer = new Thread(new ThreadStart(ThreadConsumer));
            //_threadConsumer.Start();
            #endregion Test on localhost

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

        #region Test on localhost

        private void ThreadProducerTest()
        {
            TransactionProducerTest.PublishTransactionMessage();            
        }

        private void ThreadConsumer()
        {
            int interval = 100000;
            int elapsed = 0;

            int waitTime = 1000;
            try
            {
                while (true)
                {
                    if (interval <= elapsed)
                    {
                        elapsed = 0;
                        TransactionConsumerTest.ConsumerMessage();
                    }
                    Thread.Sleep(waitTime);
                    elapsed += waitTime;
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        #endregion Test on localhost

        private void ConsumerCapturingTransaction()
        {
            int interval = 10000;
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
            _rabbitMQ.DeclareQueue("CapturingTransaction")
                       .DeclareRoutingKey("CapturingTransactionKey")
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
        
        }

    }
}
