namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using System;
    using System.Text;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class TransactionConsumerTest
    {
        private const string exchangeName = "FraudCaseExchange";
        private const string queueName = "CapturingTransaction";

        public static void ConsumerMessage()
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672 };

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel chanel = connection.CreateModel())
                {
                    chanel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

                    chanel.QueueDeclare(queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    chanel.QueueBind(queueName, exchangeName, string.Empty);

                    Console.WriteLine("Waiting for message");

                    var consumer = new EventingBasicConsumer(chanel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine("Received - {0}", message);
                    };

                    chanel.BasicConsume(queue: queueName,
                            autoAck: true,
                            consumer: consumer);
                }
            }
        }        

    }
}
