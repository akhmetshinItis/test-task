namespace Vitacore.Test.Core.Requests.Background.ProcessOutboxMessages
{
    public class ProcessOutboxMessagesResult
    {
        public int ProcessedMessagesCount { get; set; }
        public int FailedMessagesCount { get; set; }
    }
}
