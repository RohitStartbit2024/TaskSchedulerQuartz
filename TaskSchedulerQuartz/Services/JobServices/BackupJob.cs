using Quartz;

namespace TaskSchedulerQuartz.Services.JobServices
{
    public class BackupJob : IJob
    {
        private readonly ILogger<BackupJob> _logger;

        public BackupJob(ILogger<BackupJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Backup Job executed at {time}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
