using Quartz;

namespace TaskSchedulerQuartz.Jobs
{
    public class EmailJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Email Job executed at " + DateTime.Now);
            await Task.CompletedTask;
        }
    }
}
