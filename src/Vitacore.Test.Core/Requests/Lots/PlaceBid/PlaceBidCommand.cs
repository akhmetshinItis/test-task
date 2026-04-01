using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.PlaceBid;

namespace Vitacore.Test.Core.Requests.Lots.PlaceBid
{
    public class PlaceBidCommand : IRequest<PlaceBidResponse>
    {
        public PlaceBidCommand(Guid lotId, Guid userId, PlaceBidRequest request)
        {
            LotId = lotId;
            UserId = userId;
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public Guid LotId { get; }
        public Guid UserId { get; }
        public PlaceBidRequest Request { get; }
    }
}
