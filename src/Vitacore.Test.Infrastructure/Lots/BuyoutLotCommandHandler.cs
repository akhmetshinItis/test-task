using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Contracts.Requests.Lots.BuyoutLot;
using Vitacore.Test.Core;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Requests.Email.SendLotReceiptEmail;
using Vitacore.Test.Core.Requests.Lots.BuyoutLot;

namespace Vitacore.Test.Infrastructure.Lots
{
    public class BuyoutLotCommandHandler : IRequestHandler<BuyoutLotCommand, BuyoutLotResponse>
    {
        private const int MaxConcurrencyRetries = 3;

        private readonly IAppDbContext _dbContext;

        public BuyoutLotCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BuyoutLotResponse> Handle(BuyoutLotCommand request, CancellationToken cancellationToken)
        {
            for (var attempt = 0; attempt < MaxConcurrencyRetries; attempt++)
            {
                var lot = await _dbContext.TangerineLots
                    .FirstOrDefaultAsync(x => x.Id == request.LotId, cancellationToken);

                if (lot is null)
                {
                    throw new EntityNotFoundException<TangerineLot>(request.LotId);
                }

                ValidateBuyout(lot);

                var now = DateTime.UtcNow;
                var buyoutPrice = lot.BuyoutPrice!.Value;

                lot.CurrentPrice = buyoutPrice;
                lot.CurrentLeaderUserId = request.UserId;
                lot.BuyerId = request.UserId;
                lot.PurchaseType = PurchaseType.Buyout;
                lot.Status = LotStatus.Sold;
                lot.ClosedAt = now;

                _dbContext.OutboxMessages.Add(new OutboxMessage(
                    new SendLotReceiptEmailCommand(
                        request.UserId,
                        lot.Id,
                        lot.Title,
                        buyoutPrice,
                        PurchaseType.Buyout,
                        now)));

                try
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return new BuyoutLotResponse
                    {
                        LotId = lot.Id,
                        UserId = request.UserId,
                        Amount = buyoutPrice,
                        ClosedAt = now
                    };
                }
                catch (DbUpdateConcurrencyException) when (attempt < MaxConcurrencyRetries - 1)
                {
                    _dbContext.ClearChangeTracker();
                }
            }

            throw new AppException("Не удалось выполнить выкуп из-за конкурентного изменения. Повторите попытку.");
        }

        private static void ValidateBuyout(TangerineLot lot)
        {
            var now = DateTime.UtcNow;

            if (lot.Status != LotStatus.Active)
            {
                throw new AppException("Выкуп доступен только для активных лотов.");
            }

            if (!lot.BuyoutPrice.HasValue)
            {
                throw new AppException("Для этого лота выкуп недоступен.");
            }

            if (lot.CurrentPrice >= lot.BuyoutPrice.Value)
            {
                throw new AppException("Выкуп недоступен, потому что текущая ставка достигла или превысила цену выкупа.");
            }

            if (lot.AuctionStartAt > now)
            {
                throw new AppException("Аукцион для этого лота еще не начался.");
            }

            if (lot.AuctionEndAt <= now)
            {
                throw new AppException("Аукцион для этого лота уже завершен.");
            }

            if (lot.ExpirationAt <= now)
            {
                throw new AppException("Срок годности лота истек.");
            }
        }
    }
}
