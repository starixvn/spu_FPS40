namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertTransaction
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using Quartz;
    using RestSharp;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataTransformService dataTransformService;
        private const int numberOfRawTrans = 100;
        public Job()
        {
            dataTransformService = new Services.Implement.DataTransformService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var apiUrls = AppSettings.SelfHostUrl.Split(';').ToList();

                if (stagingTranId.MinStagingTransId != 0 && stagingTranId.MaxStagingTransId != 0)
                {                   
                    var stagingTrans = Enumerable.Range(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId - stagingTranId.MinStagingTransId + 1).ToList();
                    {
                        item.APIUrl = apiUrls[index];

                        return item;
                    });

                    _ = Parallel.ForEach(stagingTranIdAPI, new ParallelOptions { MaxDegreeOfParallelism = stagingTranIdAPI.Count() }, x =>
                       {
                           var rangeTrans = new StagingRawTransactionModel
                           {
                               MinStagingTransId = x.MinStagingTransId,
                               MaxStagingTransId = x.MaxStagingTransId
                           };

                           var client = new RestClient(new Uri(x.APIUrl));
                           var request = new RestRequest("api/active", Method.POST);

                           request.AddHeader("Content-type", "application/json");
                           request.AddJsonBody(rangeTrans);
                          var response = client.Execute(request);

                          if(response.ResponseStatus != ResponseStatus.Completed)
                          {
                              LogService.Error(baseUrl + " - " + response.ErrorException.Message);
                          }

                           var response = client.Execute(request);

                           if (response.ResponseStatus != ResponseStatus.Completed)
                           {
                               LogService.Error(x.APIUrl + " - " + response.ErrorException.Message);
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

            return Task.FromResult(true);
        }
    }
}
