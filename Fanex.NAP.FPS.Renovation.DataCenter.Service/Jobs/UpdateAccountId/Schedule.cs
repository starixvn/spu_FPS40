namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateAccountId
{
    using Quartz;
    using Quartz.Impl;

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
                .WithIdentity("UpdateAccountIdJob", "DataCenterGroup")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateAccountIdTrigger", "DataCenterGroup")
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
