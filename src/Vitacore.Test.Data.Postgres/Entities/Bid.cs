using Vitacore.Test.Data.Postgres.Abstractions;

namespace Vitacore.Test.Data.Postgres.Entities
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
