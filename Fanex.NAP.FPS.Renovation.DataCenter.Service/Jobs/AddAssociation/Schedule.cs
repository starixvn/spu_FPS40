using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.AddAssociation
{
    public class Schedule : ISchedule
    {
        private IScheduler scheduler;

        public virtual void Run()
        {
            var props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            var sf = new StdSchedulerFactory(props);

            scheduler = sf.GetScheduler().Result;           

            var job = JobBuilder.Create<Job>()
                .WithIdentity("AddAssociationJob", "MainDBGroup")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("AddAssociationTrigger", "MainDBGroup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);

            scheduler.Start();           
        }

        public void Shutdown()
        {
            scheduler.Shutdown(true);
        }
    }
}
