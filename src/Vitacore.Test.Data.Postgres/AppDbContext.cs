using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vitacore.Test.Core.Abstractions;
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
            ApplySoftDeleteQueryFilters(builder);
        }

        private static void ApplySoftDeleteQueryFilters(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    continue;
                }

                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var isDeletedProperty = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(compareExpression, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}
