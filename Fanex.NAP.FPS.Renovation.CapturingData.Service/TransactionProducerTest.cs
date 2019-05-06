namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{   
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using RabbitMQ.Client;
    using System;    
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Fanex.NAP.FPS.Renovation.Queue;
    using EasyNetQ;

    public class TransactionProducerTest
    {
        private const string exchangeName = "FraudCaseExchange";
        private const string queueName = "CapturingTransaction";
        private const string routingKey = "CapturingTransactionKey";

        private static EasyNetQ _rabbitMQ;

        public static void PublishTransactionMessage()
        {
            _rabbitMQ = new EasyNetQ(AppSetting.Host + ":" + AppSetting.Post,
                                       AppSetting.VirtualHost,
                                       AppSetting.PrefetchCount,
                                       AppSetting.UserName,
                                       AppSetting.Password);
            _rabbitMQ.CreateBus()
                     .DeclareExchange(AppSetting.Exchange);

            _rabbitMQ.DeclareQueue("CapturingTransaction")
                    .DeclareRoutingKey("CapturingTransactionKey")
                    .Bind();

            CapturingTransactionMessage transaction = BuildCapturingTransaction();

            Message<CapturingTransactionMessage> message = new Message<CapturingTransactionMessage>(transaction);

            _rabbitMQ.AdvanceBus.Publish<CapturingTransactionMessage>(_rabbitMQ.Exchange, routingKey, false, message);

            Console.WriteLine("Message Sent Successful");
        }

        private static CapturingTransactionMessage BuildCapturingTransaction()
        {
            CapturingTransactionMessage transaction = new CapturingTransactionMessage();
            transaction.Account = "acount 01";
            transaction.Action = "action 01";
            transaction.ActionResult = "Action Result 01";
            transaction.CaptchaResult = "Captcha Result 01";
            transaction.Device = "Device 01";

            return transaction;
        }
    
    }
}
