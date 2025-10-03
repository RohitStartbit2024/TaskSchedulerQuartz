using Quartz;

namespace TaskSchedulerQuartz.Jobs
{
    public class ReportJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Report Job executed at " + DateTime.Now);
            await Task.CompletedTask;
        }
    }
}
