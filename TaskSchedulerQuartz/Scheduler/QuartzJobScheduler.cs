using Quartz;
using TaskSchedulerQuartz.Services.JobServices;

namespace TaskSchedulerQuartz.Scheduler
{
    public static class QuartzJobScheduler
    {
        //Note: Quartz cron expressions have 6 (or 7) fields:
        //Seconds  Minutes  Hours  DayOfMonth  Month  DayOfWeek  [Year - optional]

        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator q)
        {
            // Email Job - Runs daily at 3:00 PM
            var emailJobKey = new JobKey("EmailJob");
            q.AddJob<EmailJob>(opts => opts.WithIdentity(emailJobKey));
            q.AddTrigger(opts => opts
                .ForJob(emailJobKey)
                .WithIdentity("EmailJob-trigger")
                .WithCronSchedule("0 0 15 * * ?"));   
            
            //Note: Quartz uses the server’s local time.
            //To specify a time zone,
            //.WithCronSchedule("0 0 15 * * ?", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))));
                                                   
            // Weekly Job (Every Friday at 3:00 PM)
            var backupJobKey = new JobKey("BackupJob");
            q.AddJob<BackupJob>(opts => opts.WithIdentity(backupJobKey));
            q.AddTrigger(opts => opts
                .ForJob(backupJobKey)
                .WithIdentity("BackupJob-trigger")
                .WithCronSchedule("0 0 15 ? * FRI"));

            // Monthly Job (30th of each month at 3:00 PM)
            var reportJobKey = new JobKey("ReportJob");
            q.AddJob<ReportJob>(opts => opts.WithIdentity(reportJobKey));
            q.AddTrigger(opts => opts
                .ForJob(reportJobKey)
                .WithIdentity("ReportJob-trigger")
                .WithCronSchedule("0 0 15 30 * ?"));

            // One-time Special Job (Oct 10, 2025 at 3:00 PM)
            var specialJobKey = new JobKey("SpecialJob");
            q.AddJob<SpecialJob>(opts => opts.WithIdentity(specialJobKey));
            q.AddTrigger(opts => opts
                .ForJob(specialJobKey)
                .WithIdentity("SpecialJob-trigger")
                .StartAt(new DateTimeOffset(new DateTime(2025, 10, 03, 15, 15, 0)))
                .WithSimpleSchedule(x => x.WithRepeatCount(0)));

            //for using specific timezone
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var targetTime = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2025, 10, 3, 15, 15, 0), indiaTimeZone);

            var specialJobKeyTimezoneSpecific = new JobKey("SpecialJob");
            q.AddJob<SpecialJob>(opts => opts.WithIdentity(specialJobKeyTimezoneSpecific));
            q.AddTrigger(opts => opts
                .ForJob(specialJobKeyTimezoneSpecific) // ✅ use the same key
                .WithIdentity("SpecialJob-trigger")
                .StartAt(new DateTimeOffset(targetTime)) // UTC time
                .WithSimpleSchedule(x => x.WithRepeatCount(0)));
            //

            // Last Day of Month Job (3:00 PM)
            var lastDayJobKey = new JobKey("LastDayOfMonthJob");
            q.AddJob<LastDayOfMonthJob>(opts => opts.WithIdentity(lastDayJobKey));
            q.AddTrigger(opts => opts
                .ForJob(lastDayJobKey)
                .WithIdentity("LastDayOfMonthJob-trigger")
                .WithCronSchedule("0 0 15 L * ?"));

        }
    }
}
