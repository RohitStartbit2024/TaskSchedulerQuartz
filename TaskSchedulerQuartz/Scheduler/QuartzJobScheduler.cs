using Quartz;
using TaskSchedulerQuartz.Jobs;

namespace TaskSchedulerQuartz.Scheduler
{
    public static class QuartzJobScheduler
    {
        // NOTE: Quartz cron expressions follow this pattern:
        // Seconds | Minutes | Hours | DayOfMonth | Month | DayOfWeek | [Year - optional]
        // Example: "0 0 15 * * ?" means → 3:00 PM every day.

        public static void ConfigureJobs(IServiceCollectionQuartzConfigurator q)
        {
            // ─────────────────────────────────────────────────────────────
            // DAILY JOB → Runs every day at 3:00 PM (Server Local Time)
            // ─────────────────────────────────────────────────────────────
            var emailJobKey = new JobKey("EmailJob");
            q.AddJob<EmailJob>(opts => opts.WithIdentity(emailJobKey));
            q.AddTrigger(opts => opts
                .ForJob(emailJobKey)
                .WithIdentity("EmailJob-trigger")
                .WithCronSchedule("0 0 15 * * ?"));   // Daily at 3:00 PM

            // To use a specific timezone instead of server local:
            // .WithCronSchedule("0 0 15 * * ?", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))));



            // ─────────────────────────────────────────────────────────────
            // WEEKLY JOB → Runs every Friday at 3:00 PM
            // ─────────────────────────────────────────────────────────────
            var backupJobKey = new JobKey("BackupJob");
            q.AddJob<BackupJob>(opts => opts.WithIdentity(backupJobKey));
            q.AddTrigger(opts => opts
                .ForJob(backupJobKey)
                .WithIdentity("BackupJob-trigger")
                .WithCronSchedule("0 0 15 ? * FRI")); // Every Friday at 3 PM



            // ─────────────────────────────────────────────────────────────
            // MONTHLY JOB → Runs on the 30th of every month at 3:00 PM
            // ─────────────────────────────────────────────────────────────
            var reportJobKey = new JobKey("ReportJob");
            q.AddJob<ReportJob>(opts => opts.WithIdentity(reportJobKey));
            q.AddTrigger(opts => opts
                .ForJob(reportJobKey)
                .WithIdentity("ReportJob-trigger")
                .WithCronSchedule("0 0 15 30 * ?")); // 30th day, 3 PM



            // ─────────────────────────────────────────────────────────────
            // ONE-TIME JOB → Runs only once on a specific date/time
            // ─────────────────────────────────────────────────────────────
            var specialJobKey = new JobKey("SpecialJob");
            q.AddJob<SpecialJob>(opts => opts.WithIdentity(specialJobKey));
            q.AddTrigger(opts => opts
                .ForJob(specialJobKey)
                .WithIdentity("SpecialJob-trigger")
                .StartAt(new DateTimeOffset(new DateTime(2025, 10, 03, 15, 15, 0))) // 3:15 PM, Oct 3, 2025
                .WithSimpleSchedule(x => x.WithRepeatCount(0))); // No repeat



            // ─────────────────────────────────────────────────────────────
            // ONE-TIME JOB (Timezone-Aware) → Runs at 3:15 PM IST
            // ─────────────────────────────────────────────────────────────
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            // Convert target IST time to UTC (Quartz always stores triggers in UTC)
            var targetTime = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2025, 10, 3, 15, 15, 0), indiaTimeZone);

            var specialJobKeyTimezoneSpecific = new JobKey("SpecialJobTimezoneSpecific");
            q.AddJob<SpecialJob>(opts => opts.WithIdentity(specialJobKeyTimezoneSpecific));
            q.AddTrigger(opts => opts
                .ForJob(specialJobKeyTimezoneSpecific)
                .WithIdentity("SpecialJobTimezoneSpecific-trigger")
                .StartAt(new DateTimeOffset(targetTime)) // Runs at 3:15 PM IST
                .WithSimpleSchedule(x => x.WithRepeatCount(0)));



            // ─────────────────────────────────────────────────────────────
            // LAST-DAY-OF-MONTH JOB → Runs on 28/29/30/31 depending on month
            // ─────────────────────────────────────────────────────────────
            var lastDayJobKey = new JobKey("LastDayOfMonthJob");
            q.AddJob<LastDayOfMonthJob>(opts => opts.WithIdentity(lastDayJobKey));
            q.AddTrigger(opts => opts
                .ForJob(lastDayJobKey)
                .WithIdentity("LastDayOfMonthJob-trigger")
                .WithCronSchedule("0 0 15 L * ?")); // 'L' = last day of month



            // ─────────────────────────────────────────────────────────────
            // INTERVAL JOB → Repeats every 10 seconds (no specific start)
            // ─────────────────────────────────────────────────────────────
            var intervalJobKey = new JobKey("IntervalJob");
            q.AddJob<IntervalJob>(opts => opts.WithIdentity(intervalJobKey));
            q.AddTrigger(opts => opts
                .ForJob(intervalJobKey)
                .WithIdentity("IntervalJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromSeconds(10)) // repeat every 10 sec
                    .RepeatForever()));                    // runs indefinitely



            // ─────────────────────────────────────────────────────────────
            // INTERVAL JOB (Timezone-specific start)
            // Starts 5 seconds later (based on IST), then runs every 10 seconds
            // ─────────────────────────────────────────────────────────────
            var intervalJobKey2 = new JobKey("IntervalJobIST");
            q.AddJob<IntervalJob>(opts => opts.WithIdentity(intervalJobKey2));

            q.AddTrigger(opts => opts
                .ForJob(intervalJobKey2)
                .WithIdentity("IntervalJobIST-trigger")
                .WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromSeconds(10))
                    .RepeatForever())
                .StartAt(TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.Now.AddSeconds(5),
                    TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))));
        }
    }
}
