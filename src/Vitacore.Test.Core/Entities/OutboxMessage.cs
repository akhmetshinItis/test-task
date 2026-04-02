using System.Text.Json;
using Vitacore.Test.Core.Abstractions;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Core.Entities
{
    public class OutboxMessage : BaseEntity
    {
        private const int BaseDelaySeconds = 5;
        private static readonly TimeSpan MaxDelay = TimeSpan.FromHours(12);

        public OutboxMessage(object payload)
        {
            ArgumentNullException.ThrowIfNull(payload);

            EntityType = payload.GetType().AssemblyQualifiedName
                ?? throw new InvalidOperationException("Не удалось определить тип Outbox-сообщения.");
            Payload = JsonSerializer.Serialize(payload);
            CreatedAt = DateTime.UtcNow;
            NextAttemptAt = CreatedAt;
            Status = OutboxMessageStatus.Pending;
        }

        private OutboxMessage()
        {
        }

        public string EntityType
        {
            get;
            private set => field = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException("Тип сообщения")
                : value;
        } = string.Empty;

        public string Payload
        {
            get;
            private set => field = string.IsNullOrWhiteSpace(value)
                ? throw new RequiredFieldNotSpecifiedException("Тело сообщения")
                : value;
        } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public int RetryCount { get; private set; }

        public OutboxMessageStatus Status { get; set; }

        public DateTime NextAttemptAt { get; private set; }

        public void MarkFailure(DateTime currentTime)
        {
            RetryCount++;
            Status = OutboxMessageStatus.Pending;

            const int maxExponent = 20;
            var exponent = Math.Min(RetryCount - 1, maxExponent);
            var delay = BaseDelaySeconds * Math.Pow(2, exponent);

            NextAttemptAt = currentTime.AddSeconds(Math.Min(delay, MaxDelay.TotalSeconds));
        }
    }
}
