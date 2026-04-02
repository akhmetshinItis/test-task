namespace Vitacore.Test.Contracts.Requests.Lots.GenerateTangerineLots
{
    public class GenerateTangerineLotsResponse
    {
        public int GeneratedLotsCount { get; set; }

        public IReadOnlyCollection<GeneratedTangerineLotResponse> Items { get; set; } =
            new List<GeneratedTangerineLotResponse>();
    }
}
