namespace Fanex.NAP.FPS.Renovation.CapturingData.Service.ProducerTest
{
    using Fanex.NAP.FPS.Renovation.Core;
    using System;
    using Fanex.NAP.FPS.Renovation.Queue;
    using EasyNetQ;
    using Fanex.Oneyed.Infrastructure.Models;

    class TransactionProducerTest
    {
        private const string exchangeName = "fpstransExchangeTest";
        private const string queueName = "newfpstransQueueTest";
        private const string routingKey = "fpstransMessage";
        
        public static void PublishTransactionMessage()
        {
           var _rabbitMQ = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);

            _rabbitMQ.CreateBus()
                    .DeclareExchange(exchangeName)
                    .DeclareQueue(queueName)
                    .DeclareRoutingKey(routingKey)
                    .Bind();

            try
            {
                int counter = 0;

                do
                {
                    counter++;

                    var message = new Message<MessageQueue>(BuildCapturingTransaction(), new MessageProperties() { DeliveryMode = 2, Priority = 9 });
                    
                    _rabbitMQ.AdvanceBus.Publish<MessageQueue>(_rabbitMQ.Exchange, routingKey, false, message);

                    Console.WriteLine($"Message({counter}) is published.");

                } while (counter < 10);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
