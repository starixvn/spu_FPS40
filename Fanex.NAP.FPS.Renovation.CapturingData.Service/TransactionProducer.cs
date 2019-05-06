namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using RabbitMQ.Client;
    using System;
    using System.Text;

    public class TransactionProducer
    {
        private const string exchangeName = "HelloExchange";
        private const string queueName = "HelloQueue";

        public static void PublishMessage()
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

                    string message = "Hello world. This is my first RabbitMQ message";
                    var body = Encoding.UTF8.GetBytes(message);

                    chanel.QueueBind(queueName, exchangeName, string.Empty);

                    chanel.BasicPublish(exchange: exchangeName,
                        routingKey: string.Empty,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine("Message Sent Successful - {0}", message);

                }
            }
        }
    }
}
