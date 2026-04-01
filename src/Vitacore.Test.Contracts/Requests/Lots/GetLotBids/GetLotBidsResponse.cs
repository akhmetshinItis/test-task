namespace Vitacore.Test.Contracts.Requests.Lots.GetLotBids
{
    public class GetLotBidsResponse
    {
        public int TotalCount { get; set; }
        public IReadOnlyCollection<BidListItemResponse> Items { get; set; } = Array.Empty<BidListItemResponse>();
    }
}
