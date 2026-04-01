namespace Vitacore.Test.Contracts.Requests.Lots.PlaceBid
{
    public class PlaceBidResponse
    {
        public Guid BidId { get; set; }
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentPrice { get; set; }
        public Guid? CurrentLeaderUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
