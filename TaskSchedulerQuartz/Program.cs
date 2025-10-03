using Quartz;
using TaskSchedulerQuartz.Services.JobServices;

namespace TaskSchedulerQuartz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add Quartz services
            builder.Services.AddQuartz(q =>
            {

                // Schedule EmailJob at 3:00 PM daily
                var emailJobKey = new JobKey("EmailJob");
                q.AddJob<EmailJob>(opts => opts.WithIdentity(emailJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(emailJobKey)
                    .WithIdentity("EmailJob-trigger")
                    .WithCronSchedule("0 59 14 * * ?")); // 3:00 PM IST daily

                // Schedule ReportJob at 3:01 PM daily
                var reportJobKey = new JobKey("ReportJob");
                q.AddJob<ReportJob>(opts => opts.WithIdentity(reportJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(reportJobKey)
                    .WithIdentity("ReportJob-trigger")
                    .WithCronSchedule("0 0 15 * * ?")); // 3:01 PM

                // Schedule BackupJob at 3:02 PM daily
                var backupJobKey = new JobKey("BackupJob");
                q.AddJob<BackupJob>(opts => opts.WithIdentity(backupJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(backupJobKey)
                    .WithIdentity("BackupJob-trigger")
                    .WithCronSchedule("0 1 15 * * ?")); // 3:02 PM
            });

            // Add Quartz Hosted Service
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
