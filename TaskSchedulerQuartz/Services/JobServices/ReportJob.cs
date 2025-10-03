using Quartz;

namespace TaskSchedulerQuartz.Services.JobServices
{
    public class ReportJob : IJob
    {
        private readonly ILogger<ReportJob> _logger;

        public ReportJob(ILogger<ReportJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Report Job executed at {time}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
