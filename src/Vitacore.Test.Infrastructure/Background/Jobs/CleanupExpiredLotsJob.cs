using MediatR;
using Microsoft.Extensions.Logging;
using Vitacore.Test.Core.Requests.Background.CleanupExpiredLots;

namespace Vitacore.Test.Infrastructure.Background.Jobs
{
    public class CleanupExpiredLotsJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CleanupExpiredLotsJob> _logger;

        public CleanupExpiredLotsJob(
            IMediator mediator,
            ILogger<CleanupExpiredLotsJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Starting Hangfire job {JobName}.", nameof(CleanupExpiredLotsJob));

            try
            {
                var result = await _mediator.Send(new CleanupExpiredLotsCommand());

                _logger.LogInformation(
                    "Completed Hangfire job {JobName}. DeletedLotsCount: {DeletedLotsCount}.",
                    nameof(CleanupExpiredLotsJob),
                    result.DeletedLotsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hangfire job {JobName} failed.", nameof(CleanupExpiredLotsJob));
                throw;
            }
        }
    }
}
