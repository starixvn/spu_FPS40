using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.Logging.Extensions;
using Fanex.Logging;
using Fanex.Logging.Extensions.Database;
using System.Threading;

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

            ThreadPool.QueueUserWorkItem(new WaitCallback((_) =>
            {
                Thread.Sleep(1000 * 10);

                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                timer.Interval = AppSettings.Interval;
                timer.Enabled = true;
            }));
        }

        protected override void OnStop()
        {
            timer.Dispose();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var insertResult = new DataResult();

            try
            {
                if (isRunning) return;

                isRunning = true;

                var result = dataTransformService.GetStagingRawTransaction();                

                if (result != null && result.Any())
                {
                    var chunkResult = result.Chunk(100);

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
