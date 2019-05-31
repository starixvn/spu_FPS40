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

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service
{
    public partial class Runner : ServiceBase
    {
        private static IDataTransformService dataTransformService;
        private static List<StagingRawTransaction> _rawTransactionList;
        private static Timer timer;
        private static bool isRunning;
        public Runner()
        {
            dataTransformService = new DataTransformService();
            _rawTransactionList = new List<StagingRawTransaction>();
            isRunning = false;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 10000;
            timer.Enabled = true;

            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        protected override void OnStop()
        {
            timer.Dispose();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (isRunning) return;

            isRunning = true;

            var result = dataTransformService.GetStagingRawTransaction();

            if (result != null && result.Any())
            {
                var chunkResult = result.OrderBy(x => x.StagingTransId).Chunk(100);

                foreach (var rawData in chunkResult)
                {
                    if (rawData.Any())
                    {
                        dataTransformService.InsertTransaction(rawData.ToList());
                    }
                }
            }

            isRunning = false;
        }
    }
}
