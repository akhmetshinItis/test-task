using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.GetLotBids;

namespace Vitacore.Test.Core.Requests.Lots.GetLotBids
{
    public class GetLotBidsQuery : IRequest<GetLotBidsResponse>
    {
        public GetLotBidsQuery(Guid lotId, GetLotBidsRequest request)
        {
            LotId = lotId;
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public Guid LotId { get; }
        public GetLotBidsRequest Request { get; }
    }
}
