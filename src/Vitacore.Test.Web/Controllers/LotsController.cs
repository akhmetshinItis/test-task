using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vitacore.Test.Contracts.Requests.Lots.GetLots;
using Vitacore.Test.Core.Requests.Lots.GetLots;

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
    }
}
