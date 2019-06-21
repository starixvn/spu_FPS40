using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateCustomerInfo
{
    public class Schedule : ISchedule
    {
        private IScheduler scheduler;

        public virtual void Run()
        {
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

            var sf = new StdSchedulerFactory(props);

            scheduler = sf.GetScheduler().Result;

            scheduler.Start();

            var job = JobBuilder.Create<Job>()
                .WithIdentity("UpdateCustomerInfoJob", "MainDBGroup")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateCustomerInfoTrigger", "MainDBGroup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);            
        }

        public void Shutdown()
        {
            scheduler.Shutdown(true);
        }
    }
}
