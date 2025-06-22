using Quartz;
using Quartz.Impl;
using System.Reflection.Metadata;

public class EmailJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        SendEmail();
        return Task.CompletedTask;
    }

    public void SendEmail()
    {
        // Logic gửi email sử dụng MailKit hoặc thư viện khác
    }

    public void ScheduleEmailJob()
    {
        IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        scheduler.Start().Wait();

        IJobDetail job = JobBuilder.Create<EmailJob>()
            .WithIdentity("emailJob", "group1")
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("emailTrigger", "group1")
            .StartNow()
            .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24))
            .Build();

        scheduler.ScheduleJob(job, trigger).Wait();
    }
}