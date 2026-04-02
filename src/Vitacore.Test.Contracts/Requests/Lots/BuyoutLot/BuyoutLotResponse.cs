namespace Vitacore.Test.Contracts.Requests.Lots.BuyoutLot
{
    public class BuyoutLotResponse
    {
        public Guid LotId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ClosedAt { get; set; }
    }
}
