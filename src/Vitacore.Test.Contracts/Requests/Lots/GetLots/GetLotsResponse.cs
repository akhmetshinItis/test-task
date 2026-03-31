namespace Vitacore.Test.Contracts.Requests.Lots.GetLots
{
    public class GetLotsResponse
    {
        public int TotalCount { get; set; }
        public IReadOnlyCollection<LotListItemResponse> Items { get; set; } = Array.Empty<LotListItemResponse>();
    }
}
