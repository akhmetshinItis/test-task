using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vitacore.Test.Core.Authorization;
using Vitacore.Test.Core.Requests.Background.CleanupExpiredLots;
using Vitacore.Test.Core.Requests.Background.CompleteEndedAuctions;
using Vitacore.Test.Core.Requests.Background.ProcessOutboxMessages;

namespace Vitacore.Test.Web.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class JobsController : ApiControllerBase
    {
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ProcessOutboxMessagesResult))]
        [HttpPost("process-outbox")]
        public async Task<ProcessOutboxMessagesResult> ProcessOutboxAsync(
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
            => await mediator.Send(new ProcessOutboxMessagesCommand(), cancellationToken);

        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(CompleteEndedAuctionsResult))]
        [HttpPost("complete-ended-auctions")]
        public async Task<CompleteEndedAuctionsResult> CompleteEndedAuctionsAsync(
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
            => await mediator.Send(new CompleteEndedAuctionsCommand(), cancellationToken);

        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(CleanupExpiredLotsResult))]
        [HttpPost("cleanup-expired-lots")]
        public async Task<CleanupExpiredLotsResult> CleanupExpiredLotsAsync(
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
            => await mediator.Send(new CleanupExpiredLotsCommand(), cancellationToken);
    }
}
