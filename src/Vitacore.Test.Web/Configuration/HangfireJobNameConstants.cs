namespace Vitacore.Test.Web.Configuration
{
    public static class HangfireJobNameConstants
    {
        public const string ProcessOutboxMessages = "outbox:process";
        public const string CompleteEndedAuctions = "auctions:complete-ended";
        public const string CleanupExpiredLots = "lots:cleanup-expired";
        public const string GenerateTangerineLots = "lots:generate";
    }
}
