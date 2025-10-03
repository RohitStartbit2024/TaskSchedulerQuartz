using Quartz;

namespace TaskSchedulerQuartz.Jobs
{
    public class BackupJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Backup Job executed at " + DateTime.Now);
            await Task.CompletedTask;
        }
    }
}
