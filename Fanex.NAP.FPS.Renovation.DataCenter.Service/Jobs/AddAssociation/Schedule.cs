namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.AddAssociation
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
            scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

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
