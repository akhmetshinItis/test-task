using MediatR;
using Microsoft.Extensions.Logging;
using Vitacore.Test.Core.Requests.Background.ProcessOutboxMessages;

namespace Vitacore.Test.Infrastructure.Background.Jobs
{
    public class ProcessOutboxMessagesJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProcessOutboxMessagesJob> _logger;

        public ProcessOutboxMessagesJob(
            IMediator mediator,
            ILogger<ProcessOutboxMessagesJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Starting Hangfire job {JobName}.", nameof(ProcessOutboxMessagesJob));

            try
            {
                var result = await _mediator.Send(new ProcessOutboxMessagesCommand());

                _logger.LogInformation(
                    "Completed Hangfire job {JobName}. ProcessedMessagesCount: {ProcessedMessagesCount}. FailedMessagesCount: {FailedMessagesCount}.",
                    nameof(ProcessOutboxMessagesJob),
                    result.ProcessedMessagesCount,
                    result.FailedMessagesCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hangfire job {JobName} failed.", nameof(ProcessOutboxMessagesJob));
                throw;
            }
        }
    }
}
