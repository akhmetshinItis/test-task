using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vitacore.Test.Contracts.Requests.Lots.GetLotById;
using Vitacore.Test.Contracts.Requests.Lots.GetLotBids;
using Vitacore.Test.Contracts.Requests.Lots.GetLots;
using Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots;
using Vitacore.Test.Contracts.Requests.Lots.PlaceBid;
using Vitacore.Test.Core.Authorization;
using Vitacore.Test.Core.Requests.Lots.GetLotById;
using Vitacore.Test.Core.Requests.Lots.GetLotBids;
using Vitacore.Test.Core.Requests.Lots.GetLots;
using Vitacore.Test.Core.Requests.Lots.GenerateTangerineLots;
using Vitacore.Test.Core.Requests.Lots.PlaceBid;

namespace Vitacore.Test.Web.Controllers
{
    public class LotsController : ApiControllerBase
    {
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetLotsResponse))]
        [HttpGet]
        public async Task<GetLotsResponse> GetLotsAsync(
            [FromServices] IMediator mediator,
            [FromQuery] GetLotsRequest request,
            CancellationToken cancellationToken)
            => await mediator.Send(new GetLotsQuery(request), cancellationToken);

        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetLotByIdResponse))]
        [HttpGet("{id:guid}")]
        public async Task<GetLotByIdResponse> GetLotByIdAsync(
            [FromServices] IMediator mediator,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
            => await mediator.Send(new GetLotByIdQuery(id), cancellationToken);

        [Authorize(Roles = ApplicationRoles.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GenerateTangerineLotsResponse))]
        [HttpPost("generate")]
        public async Task<GenerateTangerineLotsResponse> GenerateTangerineLotsAsync(
            [FromServices] IMediator mediator,
            [FromBody] GenerateTangerineLotsRequest request,
            CancellationToken cancellationToken)
            => await mediator.Send(
                new GenerateTangerineLotsCommand(request),
                cancellationToken);

        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetLotBidsResponse))]
        [HttpGet("{id:guid}/bids")]
        public async Task<GetLotBidsResponse> GetLotBidsAsync(
            [FromServices] IMediator mediator,
            [FromRoute] Guid id,
            [FromQuery] GetLotBidsRequest request,
            CancellationToken cancellationToken)
            => await mediator.Send(new GetLotBidsQuery(id, request), cancellationToken);

        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PlaceBidResponse))]
        [HttpPost("{id:guid}/bids")]
        public async Task<PlaceBidResponse> PlaceBidAsync(
            [FromServices] IMediator mediator,
            [FromRoute] Guid id,
            [FromBody] PlaceBidRequest request,
            CancellationToken cancellationToken)
            => await mediator.Send(new PlaceBidCommand(id, GetCurrentUserId(), request), cancellationToken);
    }
}
