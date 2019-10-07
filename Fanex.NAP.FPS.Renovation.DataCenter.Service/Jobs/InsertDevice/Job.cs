namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertDevice
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using MySql.Data.MySqlClient;
    using Quartz;
    using RestSharp;
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
                var apiUrls = AppSettings.HostPathForDevice.Split(';');

                var listOfTrans = dataCenterService.GetStagingTransIdForDevice(AppSettings.SizeOfDevice, apiUrls.Count());

                if (listOfTrans.Any())
                {
                    Core.Utilities.WriteLog("InsertDevice.log", "Start: " + DateTime.Now);
                    
                    var groupTrans = listOfTrans.GroupBy(x => x.BatchId);
                    var tasks = new List<Task>();
                    var index = 0;

                    foreach (var trans in groupTrans)
                    {                        
                        var task = Task.Run(() =>
                        {
                            return Executing(trans.ToList(), apiUrls[index >= apiUrls.Length ? 0 : index]);
                        });

                        await Task.Delay(10);

                        tasks.Add(task);

                        index++;
                    }

                    await Task.WhenAll(tasks.ToArray());

                    Console.WriteLine($"Device is inserted at {DateTime.Now}");


                    Core.Utilities.WriteLog("InsertDevice.log", "End: " + DateTime.Now);
                }
                else
                {
                    LogService.Info("There is not existing staging raw transaction for updating device.");
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

        private static async Task<IRestResponse> Executing(List<Model.DevicePackage> devices, string apiUrl)
        {

            var client = new RestClient(new Uri(apiUrl));
            var request = new RestRequest("api/active", Method.POST);

            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(devices);

            return await client.ExecuteTaskAsync(request);
        }
    }
}
