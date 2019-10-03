namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateCustomerInfo
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly IDataCenterService dataCenterService;
        private const int numberOfCustomer = 1000;        

        public Job()
        {
            dataCenterService = new DataCenterService();           
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Core.Utilities.WriteLog("UpdateCustomerInfo", "Start: " + DateTime.Now.ToLongDateString());

                var newCustomer = dataCenterService.GetNewCustomer(numberOfCustomer);

                if (newCustomer != null && newCustomer.Any())
                {
                    var customerInfo = dataCenterService.GetCustomerInfo(newCustomer);

                    if (customerInfo != null && customerInfo.Any())
                    {
                        dataCenterService.UpdateCustomerInfo(customerInfo);
                    }
                }

                Core.Utilities.WriteLog("UpdateCustomerInfo", "End: " + DateTime.Now.ToLongDateString());
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            Console.WriteLine($"Customer is updated at {DateTime.Now}");

            return Task.FromResult(true);
        }
    }
}
