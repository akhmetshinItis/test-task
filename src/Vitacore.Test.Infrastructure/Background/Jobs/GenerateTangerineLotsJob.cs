using MediatR;
using Microsoft.Extensions.Logging;
using Vitacore.Test.Core.Options;
using Vitacore.Test.Core.Requests.Background.GenerateTangerineLots;

namespace Vitacore.Test.Infrastructure.Background.Jobs
{
    public class GenerateTangerineLotsJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GenerateTangerineLotsJob> _logger;
        private readonly TangerineLotGenerationOptions _options;

        public GenerateTangerineLotsJob(
            IMediator mediator,
            ILogger<GenerateTangerineLotsJob> logger,
            TangerineLotGenerationOptions options)
        {
            _mediator = mediator;
            _logger = logger;
            _options = options;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Starting Hangfire job {JobName}.", nameof(GenerateTangerineLotsJob));

            try
            {
                var result = await _mediator.Send(new GenerateTangerineLotsBackgroundCommand(_options.JobGenerateCount));

                _logger.LogInformation(
                    "Completed Hangfire job {JobName}. GeneratedLotsCount: {GeneratedLotsCount}.",
                    nameof(GenerateTangerineLotsJob),
                    result.GeneratedLotsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hangfire job {JobName} failed.", nameof(GenerateTangerineLotsJob));
                throw;
            }
        }
    }
}
