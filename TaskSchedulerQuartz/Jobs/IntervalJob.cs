using Quartz;

namespace TaskSchedulerQuartz.Jobs
{
    public class IntervalJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Interval job executed at {DateTime.Now}");
            await Task.CompletedTask;
        }
    }
}
