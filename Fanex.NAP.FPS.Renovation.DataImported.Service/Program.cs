using EasyNetQ;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.Queue;
using Fanex.Oneyed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.DataImported.Service
{
    class Program
    {
        private static EasyNetQConnector _rabbitMQ;

        static void Main(string[] args)
        {
            _rabbitMQ = new EasyNetQConnector(AppSettings.Host + ":" + AppSettings.Post,
                                      AppSettings.VirtualHost,
                                      AppSettings.PrefetchCount,
                                      AppSettings.UserName,
                                      AppSettings.Password);

            _rabbitMQ.CreateBus()
                    .DeclareExchange(AppSettings.Exchange)
                    .DeclareQueue(AppSettings.QueueName)
                    .DeclareRoutingKey(AppSettings.RoutingKey)
                    .Bind();

            ReadDataFromConfig();

        }

        private static Random random = new Random();

        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void ReadDataFromConfig()
        {
            var files = Directory.GetFiles(@"D:\Projects\FPS 4.0\SubscriberTickets");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName == "IBO.txt")
                {


                    var dataLog = File.ReadAllLines(file);

                    if (dataLog.Length > 0)
                    {
                        foreach (var log in dataLog)
                        {
                            var splitingData = log.Split('|');
                            if (splitingData.Length >= 9)
                            {
                                var objectData = new MessageQueue()
                                {
                                    Account = RandomString(10),
                                    Action = splitingData[1],
                                    ActionResult = splitingData[2],
                                    Device = splitingData[3],
                                    IP = splitingData[4],
                                    Subscriber = splitingData[5],
                                    SubscriberDate = Convert.ToDateTime(splitingData[6]),
                                    Token = splitingData[7],
                                    TransDate = Convert.ToDateTime(splitingData[8]),
                                };

                                var message = new Message<MessageQueue>(objectData, new MessageProperties { DeliveryMode = 2, Priority = 9 });

                                _rabbitMQ.AdvanceBus.PublishAsync(_rabbitMQ.Exchange, _rabbitMQ.RoutingKey, false, message);
                            }
                        }
                    }

                    Console.WriteLine(Path.GetFileName(file));
                }
            }


        }
    }
}
