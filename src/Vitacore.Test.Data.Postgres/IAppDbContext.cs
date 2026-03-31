using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Data.Postgres.Entities;

namespace Vitacore.Test.Data.Postgres
{
    public interface IAppDbContext
    {
        DbSet<TangerineLot> TangerineLots { get; }
        DbSet<Bid> Bids { get; }
    }
}
