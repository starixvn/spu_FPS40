using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Queue;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateCustomerInfo
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private static IDataCenterService dataCenterService;
        private static EasyNetQConnector _rabbitMQ;

        public Job()
        {
            dataCenterService = new DataCenterService();

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
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var newCustomer = dataCenterService.GetNewCustomer(AppSettings.TransBatch);

                if (newCustomer != null && newCustomer.Any())
                {
                    var customerInfo = dataCenterService.GetCustomerInfo(newCustomer);

                    if (customerInfo != null && customerInfo.Any())
                    {
                        dataCenterService.UpdateCustomerInfo(customerInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            Console.WriteLine($"Customer is updated at {DateTime.Now}");

            return Task.FromResult(true);
        }
    }
}
