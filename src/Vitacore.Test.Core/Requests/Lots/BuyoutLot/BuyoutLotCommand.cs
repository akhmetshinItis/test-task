using MediatR;
using Vitacore.Test.Contracts.Requests.Lots.BuyoutLot;

namespace Vitacore.Test.Core.Requests.Lots.BuyoutLot
{
    public class BuyoutLotCommand : IRequest<BuyoutLotResponse>
    {
        public BuyoutLotCommand(Guid lotId, Guid userId)
        {
            LotId = lotId;
            UserId = userId;
        }

        public Guid LotId { get; }
        public Guid UserId { get; }
    }
}
