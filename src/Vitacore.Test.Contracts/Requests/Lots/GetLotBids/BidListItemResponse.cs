namespace Vitacore.Test.Contracts.Requests.Lots.GetLotBids
{
    public class BidListItemResponse
    {
        public Guid Id { get; set; }
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
