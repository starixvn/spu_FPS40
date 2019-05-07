namespace Fanex.NAP.FPS.Renovation.CapturingData.Service.ProducerTest
{
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using System;
    using Fanex.NAP.FPS.Renovation.Queue;
    using EasyNetQ;

    class TransactionProducerTest
    {
        private const string queueName = "newfpstransQueue";
        private const string routingKey = "fpstransMessage";

        private static EasyNetQ _rabbitMQ;

        public static void PublishTransactionMessage()
        {
            _rabbitMQ = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);
            _rabbitMQ.CreateBus()
                    .DeclareExchange(AppSetting.Exchange)
                    .DeclareQueue(queueName)
                    .DeclareRoutingKey(routingKey)
                    .Bind();

            Console.WriteLine("If you want exit, input 0");
            while (true)
            {
                Console.WriteLine("Enter input: ");
                string line = Console.ReadLine();
                if (line == "0")
                {
                    break;
                }

                CapturingTransactionMessage transaction = BuildCapturingTransaction();

                Message<CapturingTransactionMessage> message = new Message<CapturingTransactionMessage>(transaction);

                _rabbitMQ.AdvanceBus.Publish<CapturingTransactionMessage>(_rabbitMQ.Exchange, routingKey, false, message);

                Console.WriteLine("Message Sent Successful");
            }
            
        }

        private static CapturingTransactionMessage BuildCapturingTransaction()
        {
            CapturingTransactionMessage transaction = new CapturingTransactionMessage();
            transaction.Account = "acount 01";
            transaction.Action = "action 01";
            transaction.ActionResult = "Action Result 01";
            transaction.Device = "Device 01";
            transaction.Ip = "IP 01";
            transaction.Subscriber = "Subscriber 01";
            transaction.SubscriberDate = DateTime.Now;
            transaction.Token = "Token 01";
            transaction.CaptchaResult = "Captcha Result 01";

            return transaction;
        }

    }
}
