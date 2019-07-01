using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
    public class Schedule : ISchedule
    {
        private IScheduler scheduler;

        public ISchedule Pause()
        {
            scheduler.PauseAll();

            return this;
        }

        public ISchedule Create()
        {
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

            var sf = new StdSchedulerFactory(props);

            scheduler = sf.GetScheduler().Result;

            scheduler.Start();

            var job = JobBuilder.Create<Job>()
                .WithIdentity("UpdateIPRangeJob", "MainDBGroup")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateIPRangeTrigger", "MainDBGroup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);

            return this;
        }

        public ISchedule Shutdown()
        {
            scheduler.Shutdown(true);

            return this;
        }

        public ISchedule Start()
        {
            scheduler.Start();

            return this;
        }
    }
}
