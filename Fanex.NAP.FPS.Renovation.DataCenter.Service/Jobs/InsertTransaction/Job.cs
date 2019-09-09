namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertTransaction
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Newtonsoft.Json;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataTransformService dataTransformService;
        private const int numberOfRawTrans = 1000;
        private const int chunkOfTrans = numberOfRawTrans / 10;
        private readonly List<string> apiUrls;
        public Job()
        {
            dataTransformService = new Services.Implement.DataTransformService();

            apiUrls = new List<string> {
                "http://10.23.10.11:9000/api/active"               
            };
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var index = 0;
                var stagingTranId = dataTransformService.GetStagingRawTransaction(0, numberOfRawTrans);

                if (stagingTranId.MinStagingTransId != 0 && stagingTranId.MaxStagingTransId != 0)
                {
                    var stagingTrans = Enumerable.Range(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId).ToList();

                    var chunkStagingTrans = stagingTrans.Chunk(chunkOfTrans);

                    _ = Parallel.ForEach(chunkStagingTrans, new ParallelOptions { MaxDegreeOfParallelism = apiUrls.Count }, x =>
                      {
                          using (var client = new HttpClient())
                          {
                              var content = new StringContent(JsonConvert.SerializeObject(x), Encoding.UTF8, "application/json");                                                           

                              client.PostAsync(apiUrls.ElementAt(index), content).ConfigureAwait(true);                                                           

                              if (index >= apiUrls.Count)
                              {
                                  index = 0;
                              }
                              else
                              {
                                  index++;
                              }
                          }
                      });
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            return Task.FromResult(false);
        }
    }
}
