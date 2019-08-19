namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateCustomerInfo
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly IDataCenterService dataCenterService;
        

        public Job()
        {
            dataCenterService = new DataCenterService();           
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
