using MediatR;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core.Enums;

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

            var soldLotsCount = await _dbContext.TangerineLots
                .Where(x => x.Status == LotStatus.Active && x.AuctionEndAt <= now && x.CurrentLeaderUserId.HasValue)
                .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.Status, LotStatus.Sold)
                        .SetProperty(x => x.BuyerId, x => x.CurrentLeaderUserId)
                        .SetProperty(x => x.PurchaseType, PurchaseType.AuctionWin)
                        .SetProperty(x => x.ClosedAt, now),
                    cancellationToken);

            var completedWithoutWinnerLotsCount = await _dbContext.TangerineLots
                .Where(x => x.Status == LotStatus.Active && x.AuctionEndAt <= now && !x.CurrentLeaderUserId.HasValue)
                .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.Status, LotStatus.CompletedWithoutWinner)
                        .SetProperty(x => x.BuyerId, x => null)
                        .SetProperty(x => x.PurchaseType, x => null)
                        .SetProperty(x => x.ClosedAt, now),
                    cancellationToken);

            return new CompleteEndedAuctionsResult
            {
                SoldLotsCount = soldLotsCount,
                CompletedWithoutWinnerLotsCount = completedWithoutWinnerLotsCount,
                ProcessedLotsCount = soldLotsCount + completedWithoutWinnerLotsCount
            };
        }
    }
}
