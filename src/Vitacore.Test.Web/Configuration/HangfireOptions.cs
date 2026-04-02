namespace Vitacore.Test.Web.Configuration
{
    public class HangfireOptions
    {
        public const string SectionName = "Hangfire";

        public bool EnableDashboard { get; set; } = true;
        public string ConnectionString { get; set; } = string.Empty;
        public string DashboardPath { get; set; } = "/hangfire";
        public int WorkerCount { get; set; } = 5;
        public string ProcessOutboxMessagesCron { get; set; } = "* * * * *";
        public string CompleteEndedAuctionsCron { get; set; } = "* * * * *";
        public string CleanupExpiredLotsCron { get; set; } = "0 0 * * *";
        public string GenerateTangerineLotsCron { get; set; } = "0 * * * *";
    }
}
