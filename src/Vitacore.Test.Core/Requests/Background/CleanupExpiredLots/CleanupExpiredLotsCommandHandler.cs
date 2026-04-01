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

            var deletedLotsCount = await _dbContext.TangerineLots
                .Where(x => x.ExpirationAt < now)
                .ExecuteDeleteAsync(cancellationToken);

            return new CleanupExpiredLotsResult
            {
                DeletedLotsCount = deletedLotsCount
            };
        }
    }
}
