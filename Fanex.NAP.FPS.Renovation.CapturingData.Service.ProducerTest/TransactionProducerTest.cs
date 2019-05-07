namespace Fanex.NAP.FPS.Renovation.CapturingData.Service.ProducerTest
{
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using System;
    using Fanex.NAP.FPS.Renovation.Queue;
    using EasyNetQ;
    using Fanex.Oneyed.Infrastructure.Models;

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

                MessageQueue transaction = BuildCapturingTransaction();

                Message<MessageQueue> message = new Message<MessageQueue>(transaction);

                _rabbitMQ.AdvanceBus.Publish<MessageQueue>(_rabbitMQ.Exchange, routingKey, false, message);

                Console.WriteLine("Message Sent Successful");
            }
        }

        private static MessageQueue BuildCapturingTransaction()
        {
            MessageQueue transaction = new MessageQueue();
            
            transaction.Account = "acount 01";
            transaction.Action = "action 01";
            transaction.ActionResult = "Action Result 01";
            transaction.Device = "Device 01";
            transaction.IP = "IP 01";
            transaction.Subscriber = "Subscriber 01";
            transaction.SubscriberDate = DateTime.Now;
            transaction.Token = "Token 01";

            return transaction;
        }
    }  
}
