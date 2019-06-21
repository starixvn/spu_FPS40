using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EasyNetQ;
using Fanex.Data;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.Queue;
using Quartz.Util;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service
{
    public partial class Runner : ServiceBase
    {
        private static List<ISchedule> schedulers;

        public Runner()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            schedulers = new List<ISchedule>();

            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (new List<Type>(type.GetInterfaces()).Contains(typeof(ISchedule)))
                {
                    var scheduler = ObjectUtils.InstantiateType<ISchedule>(type);

                    scheduler.Create().Start();

                    schedulers.Add(scheduler);
                }
            }

            LogService.Info($"Data Center service is STARTED at {DateTime.Now}");
        }

        protected override void OnStop()
        {
            schedulers.ForEach(x => x.Shutdown());

            LogService.Info($"Data Center service is STOPED at {DateTime.Now}");
        }

        protected override void OnShutdown()
        {
            schedulers.ForEach(x => x.Shutdown());
        }
    }
}
