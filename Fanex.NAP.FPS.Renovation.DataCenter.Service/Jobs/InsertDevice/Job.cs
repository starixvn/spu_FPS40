namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertDevice
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using MySql.Data.MySqlClient;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataCenterService dataCenterService;
        
        public Job()
        {
            dataCenterService = new Services.Implement.DataCenterService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var listOfTrans = dataCenterService.GetStagingTransIdForDevice(AppSettings.SizeOfDevice);

                if (listOfTrans.Any())
                {
                    Core.Utilities.WriteLog("InsertDevice.log", "Start: " + DateTime.Now);

                    var transGroup = listOfTrans.Chunk(AppSettings.SizeOfDevice / 10);

                    var tasks = new List<Task>();

                    foreach (var trans in transGroup)
                    {
                        var task = Task.Run(() =>
                        {
                            dataCenterService.InsertDevice(trans.ToList());
                        });

                        await Task.Delay(10);

                        tasks.Add(task);                        
                    }

                    await Task.WhenAll(tasks.ToArray());

                    Console.WriteLine($"Device is inserted at {DateTime.Now}");

                    Core.Utilities.WriteLog("InsertDevice.log", "End: " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                if (((MySqlException)ex.InnerException).Number != 1213)
                {
                    var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                    LogService.Error(logMessage);
                }                    
            }             
        }
    }
}
