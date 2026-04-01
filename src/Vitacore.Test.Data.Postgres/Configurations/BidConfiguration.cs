using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Data.Postgres.Identity;

namespace Vitacore.Test.Data.Postgres.Configurations
{
    public class BidConfiguration : BaseEntityConfiguration<Bid>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable(tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("ck_bids_amount_positive", "\"amount\" > 0");
            });

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
