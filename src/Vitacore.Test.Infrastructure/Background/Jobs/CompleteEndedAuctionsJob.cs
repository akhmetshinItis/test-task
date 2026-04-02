using MediatR;
using Microsoft.Extensions.Logging;
using Vitacore.Test.Core.Requests.Background.CompleteEndedAuctions;

namespace Vitacore.Test.Infrastructure.Background.Jobs
{
    public class CompleteEndedAuctionsJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompleteEndedAuctionsJob> _logger;

        public CompleteEndedAuctionsJob(
            IMediator mediator,
            ILogger<CompleteEndedAuctionsJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Starting Hangfire job {JobName}.", nameof(CompleteEndedAuctionsJob));

            try
            {
                var result = await _mediator.Send(new CompleteEndedAuctionsCommand());

                _logger.LogInformation(
                    "Completed Hangfire job {JobName}. ProcessedLotsCount: {ProcessedLotsCount}. SoldLotsCount: {SoldLotsCount}. CompletedWithoutWinnerLotsCount: {CompletedWithoutWinnerLotsCount}.",
                    nameof(CompleteEndedAuctionsJob),
                    result.ProcessedLotsCount,
                    result.SoldLotsCount,
                    result.CompletedWithoutWinnerLotsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hangfire job {JobName} failed.", nameof(CompleteEndedAuctionsJob));
                throw;
            }
        }
    }
}
