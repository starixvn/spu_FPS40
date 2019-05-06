namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using System.ServiceProcess;
    using System.Threading;
    using EasyNetQ;
    using Fanex.NAP.FPS.Renovation.CapturingData.Service.Services;
    using RabbitMQ.Client;
    //using Fanex.NAP.FPS.Renovation.Queue;

    public partial class Runner : ServiceBase
    {
        private CapturingDataService _capturingDataService;
        private Thread _threadProducerTest = null;
        private Thread _threadConsumer = null;        

        public Runner()
        {
            InitializeComponent();

            _capturingDataService = new CapturingDataService();
            //_rabbitMq = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
            //                           AppSetting.VirtualHost,
            //                           AppSetting.PrefetchCount,
            //                           AppSetting.UserName,
            //                           AppSetting.Password);
        }

        protected override void OnStart(string[] args)
        {
            // test transaction producer
            _threadProducerTest = new Thread(new ThreadStart(ThreadProducerTest));
            _threadProducerTest.Start();

            //_threadConsumer = new Thread(new ThreadStart(ThreadConsumer));
            _threadConsumer.Start();
        }

        protected override void OnStop()
        {

        }

        private void ThreadProducerTest()
        {
            TransactionProducer.PublishMessage();            
        }

        private void ThreadConsumer()
        {
            int interval = 5000;
            int elapsed = 0;

            int waitTime = 1000;
            try
            {
                while (true)
                {
                    if (interval >= elapsed)
                    {
                        elapsed = 0;
                        TransactionConsumer.ConsumerMessage();
                    }
                    Thread.Sleep(waitTime);
                    elapsed += waitTime;
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

    }
}
