using Quartz;
using TaskSchedulerQuartz.Scheduler;
using TaskSchedulerQuartz.Services.JobServices;

namespace TaskSchedulerQuartz
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Quartz
            builder.Services.AddQuartz(q =>
            {
                QuartzJobScheduler.ConfigureJobs(q);
            });

            builder.Services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });
            //

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
