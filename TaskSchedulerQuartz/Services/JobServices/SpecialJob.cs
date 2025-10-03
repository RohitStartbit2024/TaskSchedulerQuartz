using Quartz;

namespace TaskSchedulerQuartz.Services.JobServices
{
    public class SpecialJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Special job executed at " + DateTime.Now);
            await Task.CompletedTask;
        }
    }
}
