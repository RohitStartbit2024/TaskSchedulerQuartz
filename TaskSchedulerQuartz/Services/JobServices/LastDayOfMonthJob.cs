using Quartz;

namespace TaskSchedulerQuartz.Services.JobServices
{
    public class LastDayOfMonthJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("📅 Last day of month job executed at " + DateTime.Now);
            await Task.CompletedTask;
        }
    }
}
