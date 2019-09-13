using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement;
using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service
{
    public class ActiveController : ApiController
    {
        private readonly IDataTransformService dataTransformService;
        private const int maxDegreeOfParallelism = 3;
        public ActiveController()
        {
            dataTransformService = new DataTransformService();
        }

        public void Post()
        {
            var requestContent = Request.Content;
            var jsonContent = requestContent.ReadAsStringAsync().Result;

            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                var result = JsonConvert.DeserializeObject<StagingRawTransactionModel>(jsonContent);

                Process(result);
            }
            else
            {
                LogService.Error("Request content is empty.");
            }
        }

        private void Process(StagingRawTransactionModel model)
        {
            try
            {
                if (model.MinStagingTransId != 0 && model.MaxStagingTransId != 0)
                {
                    dataTransformService.UnhandleSubscriberAccountProcessing(model.MinStagingTransId, model.MaxStagingTransId);

                    Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism },
                    () =>
                    {
                        dataTransformService.UserAgentProcessing(model.MinStagingTransId, model.MaxStagingTransId);
                    },
                    () =>
                    {
                        dataTransformService.AccountProcessing(model.MinStagingTransId, model.MaxStagingTransId);
                    },
                    () =>
                    {
                        dataTransformService.TransactionProcessing(model.MinStagingTransId, model.MaxStagingTransId);

                    });

                    dataTransformService.DeviceProcessing(model.MinStagingTransId, model.MaxStagingTransId);

                    dataTransformService.CompleteProcessing(model.MinStagingTransId, model.MaxStagingTransId);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                    .ToString();

                LogService.Error(logMessage);
            }
        }
    }
}
