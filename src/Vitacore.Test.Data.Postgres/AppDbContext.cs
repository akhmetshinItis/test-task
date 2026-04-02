using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Data.Postgres.Identity;

namespace Vitacore.Test.Data.Postgres
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IAppDbContext
    {
        public DbSet<TangerineLot> TangerineLots { get; set; } = null!;
        public DbSet<Bid> Bids { get; set; } = null!;
        public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

        public void ClearChangeTracker()
            => ChangeTracker.Clear();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
