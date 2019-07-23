namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
    using Quartz;
    using Quartz.Impl;
    using System.Collections.Specialized;

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
            //NameValueCollection props = new NameValueCollection
            //    {
            //        { "quartz.serializer.type", "binary" }
            //    };

            //var sf = new StdSchedulerFactory(props);

            //scheduler = sf.GetScheduler().Result;
            scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;                        

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
