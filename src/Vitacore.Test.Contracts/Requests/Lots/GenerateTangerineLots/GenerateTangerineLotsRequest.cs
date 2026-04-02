namespace Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots
{
    public class GenerateTangerineLotsRequest
    {
        public int Count { get; set; } = 1;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal StartPrice { get; set; }
        public decimal BuyoutPrice { get; set; }
        public DateTime AuctionStartAt { get; set; }
        public DateTime AuctionEndAt { get; set; }
        public DateTime ExpirationAt { get; set; }
    }
}
