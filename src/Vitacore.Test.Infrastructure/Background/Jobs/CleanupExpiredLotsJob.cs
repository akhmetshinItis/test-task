using MediatR;
using Vitacore.Test.Core.Requests.Background.CleanupExpiredLots;

namespace Vitacore.Test.Infrastructure.Background.Jobs
{
    public class CleanupExpiredLotsJob
    {
        private readonly IMediator _mediator;

        public CleanupExpiredLotsJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task ExecuteAsync()
            => _mediator.Send(new CleanupExpiredLotsCommand());
    }
}
