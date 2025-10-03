using Quartz;
using TaskSchedulerQuartz.Services.JobServices;

namespace TaskSchedulerQuartz.Scheduler
{
    public static class QuartzJobScheduler
    {
        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator q)
        {
            // Email Job - Runs daily at 3:00 PM
            var emailJobKey = new JobKey("EmailJob");
            q.AddJob<EmailJob>(opts => opts.WithIdentity(emailJobKey));
            q.AddTrigger(opts => opts
                .ForJob(emailJobKey)
                .WithIdentity("EmailJob-trigger")
                .WithCronSchedule("0 7 15 * * ?"));

            // Report Job - Runs daily at 3:01 PM
            var reportJobKey = new JobKey("ReportJob");
            q.AddJob<ReportJob>(opts => opts.WithIdentity(reportJobKey));
            q.AddTrigger(opts => opts
                .ForJob(reportJobKey)
                .WithIdentity("ReportJob-trigger")
                .WithCronSchedule("0 8 15 * * ?"));

            // Backup Job - Runs daily at 3:02 PM
            var backupJobKey = new JobKey("BackupJob");
            q.AddJob<BackupJob>(opts => opts.WithIdentity(backupJobKey));
            q.AddTrigger(opts => opts
                .ForJob(backupJobKey)
                .WithIdentity("BackupJob-trigger")
                .WithCronSchedule("0 9 15 * * ?"));
        }
    }
}
