using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Core.Enums;
using Vitacore.Test.Core.Requests.Email.SendLotReceiptEmail;

namespace Vitacore.Test.Core.Requests.Background.CompleteEndedAuctions
{
    public class CompleteEndedAuctionsCommandHandler : IRequestHandler<CompleteEndedAuctionsCommand, CompleteEndedAuctionsResult>
    {
        private readonly IAppDbContext _dbContext;

        public CompleteEndedAuctionsCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CompleteEndedAuctionsResult> Handle(CompleteEndedAuctionsCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var completedLots = await _dbContext.TangerineLots
                .Where(x => x.Status == LotStatus.Active && x.AuctionEndAt <= now)
                .ToListAsync(cancellationToken);

            var soldLotsCount = 0;
            var completedWithoutWinnerLotsCount = 0;

            foreach (var lot in completedLots)
            {
                lot.ClosedAt = now;

                if (lot.CurrentLeaderUserId.HasValue)
                {
                    lot.Status = LotStatus.Sold;
                    lot.BuyerId = lot.CurrentLeaderUserId;
                    lot.PurchaseType = PurchaseType.AuctionWin;

                    _dbContext.OutboxMessages.Add(new OutboxMessage(
                        new SendLotReceiptEmailCommand(
                            lot.CurrentLeaderUserId.Value,
                            lot.Id,
                            lot.Title,
                            lot.CurrentPrice,
                            PurchaseType.AuctionWin,
                            now)));

                    soldLotsCount++;
                    continue;
                }

                lot.Status = LotStatus.CompletedWithoutWinner;
                lot.BuyerId = null;
                lot.PurchaseType = null;
                completedWithoutWinnerLotsCount++;
            }

            if (completedLots.Count > 0)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return new CompleteEndedAuctionsResult
            {
                SoldLotsCount = soldLotsCount,
                CompletedWithoutWinnerLotsCount = completedWithoutWinnerLotsCount,
                ProcessedLotsCount = soldLotsCount + completedWithoutWinnerLotsCount
            };
        }
    }
}
