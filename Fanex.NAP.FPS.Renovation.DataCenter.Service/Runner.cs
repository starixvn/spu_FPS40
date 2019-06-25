namespace Fanex.NAP.FPS.Renovation.DataCenter.Service
{
    using Fanex.Logging;
    using Quartz.Util;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ServiceProcess;

    public partial class Runner : ServiceBase
    {
        private List<ISchedule> schedulers;

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
