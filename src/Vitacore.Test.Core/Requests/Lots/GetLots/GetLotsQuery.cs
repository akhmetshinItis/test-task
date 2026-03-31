using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.GetLots;

namespace Vitacore.Test.Core.Requests.Lots.GetLots
{
    public class GetLotsQuery : IRequest<GetLotsResponse>
    {
        public GetLotsQuery(GetLotsRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public GetLotsRequest Request { get; }
    }
}
