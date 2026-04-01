using MediatR;
using Vitacore.Test.Core.Requests.Background.CompleteEndedAuctions;

namespace Vitacore.Test.Infrastructure.Background.Jobs
{
    public class CompleteEndedAuctionsJob
    {
        private readonly IMediator _mediator;

        public CompleteEndedAuctionsJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task ExecuteAsync()
            => _mediator.Send(new CompleteEndedAuctionsCommand());
    }
}
