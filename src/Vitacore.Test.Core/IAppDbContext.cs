using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core.Entities;

namespace Vitacore.Test.Core
{
    public interface IAppDbContext
    {
        DbSet<TangerineLot> TangerineLots { get; }
        DbSet<Bid> Bids { get; }
        DbSet<OutboxMessage> OutboxMessages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void ClearChangeTracker();
    }
}
