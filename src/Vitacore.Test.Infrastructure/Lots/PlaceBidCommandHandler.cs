using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Contracts.Requests.Lots.PlaceBid;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Requests.Lots.PlaceBid;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Data.Postgres.Entities;
using Vitacore.Test.Data.Postgres.Enums;

namespace Vitacore.Test.Infrastructure.Lots
{
    public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, PlaceBidResponse>
    {
        private const int MaxConcurrencyRetries = 3;

        private readonly IAppDbContext _dbContext;

        public PlaceBidCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlaceBidResponse> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
        {
            if (request.Request.Amount <= 0)
            {
                throw new AppException("Ставка должна быть больше нуля.");
            }

            for (var attempt = 0; attempt < MaxConcurrencyRetries; attempt++)
            {
                var lot = await _dbContext.TangerineLots
                    .FirstOrDefaultAsync(x => x.Id == request.LotId, cancellationToken);

                if (lot is null)
                {
                    throw new EntityNotFoundException<TangerineLot>(request.LotId);
                }

                ValidateBid(lot, request.Request.Amount);

                var bid = new Bid
                {
                    LotId = lot.Id,
                    UserId = request.UserId,
                    Amount = request.Request.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                lot.CurrentPrice = request.Request.Amount;
                lot.CurrentLeaderUserId = request.UserId;

                _dbContext.Bids.Add(bid);

                try
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return new PlaceBidResponse
                    {
                        BidId = bid.Id,
                        LotId = lot.Id,
                        UserId = request.UserId,
                        Amount = bid.Amount,
                        CurrentPrice = lot.CurrentPrice,
                        CurrentLeaderUserId = lot.CurrentLeaderUserId,
                        CreatedAt = bid.CreatedAt
                    };
                }
                catch (DbUpdateConcurrencyException) when (attempt < MaxConcurrencyRetries - 1)
                {
                    _dbContext.ClearChangeTracker();
                }
            }

            throw new AppException("Не удалось сохранить ставку из-за конкурентного изменения. Повторите попытку.");
        }

        private static void ValidateBid(TangerineLot lot, decimal amount)
        {
            var now = DateTime.UtcNow;

            if (lot.Status != LotStatus.Active)
            {
                throw new AppException("Ставки доступны только для активных лотов.");
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

            if (amount <= lot.CurrentPrice)
            {
                throw new AppException("Ставка должна быть больше текущей цены.");
            }
        }
    }
}
