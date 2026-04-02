using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;

namespace Vitacore.Test.Core.Requests.Background.ProcessOutboxMessages
{
    public class ProcessOutboxMessagesCommandHandler : IRequestHandler<ProcessOutboxMessagesCommand, ProcessOutboxMessagesResult>
    {
        private const int BatchSize = 100;

        private readonly IAppDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ILogger<ProcessOutboxMessagesCommandHandler> _logger;

        public ProcessOutboxMessagesCommandHandler(
            IAppDbContext dbContext,
            IMediator mediator,
            ILogger<ProcessOutboxMessagesCommandHandler> logger)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ProcessOutboxMessagesResult> Handle(ProcessOutboxMessagesCommand request, CancellationToken cancellationToken)
        {
            var result = new ProcessOutboxMessagesResult();

            while (true)
            {
                var now = DateTime.UtcNow;

                var outboxMessages = await _dbContext.OutboxMessages
                    .Where(x => x.NextAttemptAt <= now)
                    .OrderBy(x => x.CreatedAt)
                    .ThenBy(x => x.Id)
                    .Take(BatchSize)
                    .ToListAsync(cancellationToken);

                if (outboxMessages.Count == 0)
                {
                    break;
                }

                foreach (var outboxMessage in outboxMessages)
                {
                    try
                    {
                        outboxMessage.Status = OutboxMessageStatus.Processing;
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        var messageType = Type.GetType(outboxMessage.EntityType)
                            ?? throw new AppException("Не удалось определить тип Outbox-сообщения.");

                        var message = JsonSerializer.Deserialize(outboxMessage.Payload, messageType)
                            ?? throw new AppException("Не удалось десериализовать Outbox-сообщение.");

                        await _mediator.Send(message, cancellationToken);

                        _dbContext.OutboxMessages.Remove(outboxMessage);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        result.ProcessedMessagesCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при обработке Outbox-сообщения {MessageId}.", outboxMessage.Id);

                        outboxMessage.MarkFailure(DateTime.UtcNow);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        result.FailedMessagesCount++;
                    }
                }
            }

            return result;
        }
    }
}
