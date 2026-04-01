namespace Vitacore.Test.Core.Requests.Background.CompleteEndedAuctions
{
    public class CompleteEndedAuctionsResult
    {
        public int ProcessedLotsCount { get; set; }
        public int SoldLotsCount { get; set; }
        public int CompletedWithoutWinnerLotsCount { get; set; }
    }
}
