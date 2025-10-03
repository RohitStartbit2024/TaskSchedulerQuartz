using Quartz;

namespace TaskSchedulerQuartz.Services.JobServices
{
    public class EmailJob : IJob
    {
        private readonly ILogger<EmailJob> _logger;

        public EmailJob(ILogger<EmailJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Email Job executed at {time}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
