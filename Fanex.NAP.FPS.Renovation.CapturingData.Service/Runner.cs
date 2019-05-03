namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using System.ServiceProcess;
    using System.Text;
    using EasyNetQ;
    using Fanex.NAP.FPS.Renovation.CapturingData.Service.Services;
    using RabbitMQ.Client;
    //using Fanex.NAP.FPS.Renovation.Queue;    

    public partial class Runner : ServiceBase
    {
        private static CapturingDataService _capturingDataService;
        //private static EasyNetQ _rabbitMq;

        public EasyNetQ.Topology.IExchange Exchange { get; set; }

        private EasyNetQ.Topology.IBinding Binding { get; set; }
           
        public EasyNetQ.Topology.IQueue Queue { get; set; }

        public Runner()
        {
            InitializeComponent();

            _capturingDataService = new CapturingDataService();
            //_rabbitMq = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
            //                           AppSetting.VirtualHost,
            //                           AppSetting.PrefetchCount,
            //                           AppSetting.UserName,
            //                           AppSetting.Password);

            //_rabbitMq.CreateBus().DeclareExchange(AppSetting.Exchange);
          
            string localhost = "host=localhost";

            // create bus
            var bus = EasyNetQ.RabbitHutch.CreateBus(localhost);
       
            // create exchange
            ExchangeDeclare(bus);

            // create queue
            QueueDeclare(bus);

            // bind queue
            //QueueBind(bus);

            // publish message
            //PublishMessage(bus);

            // Get message
            ReceiverMessage(bus);
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }

        private void ExchangeDeclare(EasyNetQ.IBus bus)
        {
            Exchange = bus.Advanced.ExchangeDeclare("demoExchange", ExchangeType.Direct);       
        }

        private void QueueDeclare(EasyNetQ.IBus bus)
        {
            Queue = bus.Advanced.QueueDeclare("demoqueue");
        }

        private void QueueBind(EasyNetQ.IBus bus)
        {
            Binding = bus.Advanced.Bind(Exchange, Queue, "directexchange_key");
        }

        private void PublishMessage(EasyNetQ.IBus bus)
        {
            var message = new Message<string>("Direct message");

            bus.Advanced.PublishAsync(Exchange, "directexchange_key", false, message);
        }

        private void ReceiverMessage(EasyNetQ.IBus bus)
        {
            bus.Advanced.Consume(Queue, x => x.Add<string>((message, info) =>
            {
                HandleMessage(message.Body);
            }));
        }

        private void HandleMessage(string message)
        {

        }

    }
}
