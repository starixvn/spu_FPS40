using EasyNetQ;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.Queue;
using Fanex.Oneyed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Fanex.NAP.FPS.Renovation.DataImported.Service
{
   static class Program
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

        static void ReadDataFromConfig()
        {
            var files = Directory.GetFiles(@"D:\Projects\FPS 4.0\SubscriberTickets");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
               

                if (fileNameWithoutExtension == "IBO")
                {
                    Utilities.DeleteLog(fileName);

                    var dataLog = File.ReadAllLines(file);

                    if (dataLog.Length > 0)
                    {
                        Executing(fileName, fileNameWithoutExtension, dataLog);
                    }

                    Console.WriteLine(Path.GetFileName(file));
                }
            }
        }

        private static void Executing(string fileName, string fileNameWithoutExtension,  string[] dataLog)
        {
            var counter = 0;

            foreach (var log in dataLog)
            {
                var splitingData = log.Split('|');

                if (splitingData.Length >= 9)
                {
                    counter++;

                    splitingData[0] = fileNameWithoutExtension + "_" + counter;

                    Utilities.WriteLog(fileName, string.Join("|", splitingData));

                    var objectData = new MessageQueue
                    {
                        Account = splitingData[0],
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
                else
                {
                    Console.WriteLine(log);
                }
            }            
        }
    }
}
