using Vitacore.Test.Core.Abstractions;

namespace Vitacore.Test.Core.Entities
{
    public class Bid : BaseEntity
    {
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }

        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public TangerineLot Lot { get; set; } = null!;
    }
}
