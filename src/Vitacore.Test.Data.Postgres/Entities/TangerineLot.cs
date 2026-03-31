using Vitacore.Test.Data.Postgres.Abstractions;
using Vitacore.Test.Data.Postgres.Enums;

namespace Vitacore.Test.Data.Postgres.Entities
{
    public class TangerineLot : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;

        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal? BuyoutPrice { get; set; }

        public DateTime AuctionStartAt { get; set; }
        public DateTime AuctionEndAt { get; set; }
        public DateTime ExpirationAt { get; set; }

        public LotStatus Status { get; set; }

        public Guid? CurrentLeaderUserId { get; set; }
        public Guid? BuyerId { get; set; }
        public PurchaseType? PurchaseType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public uint Version { get; set; }

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}
