using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Vitacore.Test.Core.Requests.Background.CleanupExpiredLots
{
    public class CleanupExpiredLotsCommandHandler : IRequestHandler<CleanupExpiredLotsCommand, CleanupExpiredLotsResult>
    {
        private readonly IAppDbContext _dbContext;

        public CleanupExpiredLotsCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CleanupExpiredLotsResult> Handle(CleanupExpiredLotsCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var expiredLots = await _dbContext.TangerineLots
                .Where(x => x.ExpirationAt < now)
                .ToListAsync(cancellationToken);

            foreach (var lot in expiredLots)
            {
                lot.MarkAsDeleted(now);
            }

            if (expiredLots.Count > 0)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return new CleanupExpiredLotsResult
            {
                DeletedLotsCount = expiredLots.Count
            };
        }
    }
}
