namespace Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots
{
    public class GeneratedTangerineLotResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal? BuyoutPrice { get; set; }
        public DateTime AuctionStartAt { get; set; }
        public DateTime AuctionEndAt { get; set; }
        public DateTime ExpirationAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? CurrentLeaderUserId { get; set; }
        public Guid? BuyerId { get; set; }
        public string? PurchaseType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
