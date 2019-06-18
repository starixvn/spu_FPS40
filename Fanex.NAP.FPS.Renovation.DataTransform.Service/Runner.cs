using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement;
using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service
{
    public partial class Runner : ServiceBase
    {
        private static IDataTransformService dataTransformService;
        private static System.Timers.Timer timer;
        private static bool isRunning;
        public Runner()
        {
            dataTransformService = new DataTransformService();
            isRunning = false;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = AppSettings.Interval;
            timer.Enabled = true;

            LogService.Info($"Data Transform service is STARTED at {DateTime.Now}");
        }

        protected override void OnStop()
        {
            timer.Dispose();

            LogService.Info($"Data Transform service is STOPED at {DateTime.Now}");
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var insertResult = new DataResult();

            try
            {
                if (isRunning) return;

                isRunning = true;

                var result = dataTransformService.GetStagingRawTransaction(AppSettings.TransBatch);
                
                if (result != null && result.Any())
                {
                    var chunkSize = AppSettings.TransBatch / 10;
                    var chunkResult = result.Chunk(chunkSize);

                    foreach (var rawData in chunkResult)
                    {
                        if (rawData.Any())
                        {
                           insertResult = dataTransformService.InsertTransaction(rawData.ToList());                           
                        }
                    }
                }                

                isRunning = false;
            }
            catch (Exception ex)
            {                
                var errorHanler = new Dictionary<string, object>();
                errorHanler.Add("ErrorCode", insertResult.ErrorCode);
                errorHanler.Add("ErrorMsg", insertResult.ErrorMessage);

                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", errorHanler)
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))                    
                    .ToString();

                LogService.Error(logMessage);

                isRunning = false;
            }
        }
    }
}
