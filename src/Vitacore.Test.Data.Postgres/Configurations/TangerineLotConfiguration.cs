using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitacore.Test.Core.Entities;
using Vitacore.Test.Data.Postgres.Identity;

namespace Vitacore.Test.Data.Postgres.Configurations
{
    public class TangerineLotConfiguration : BaseEntityConfiguration<TangerineLot>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TangerineLot> builder)
        {
            builder.ToTable(tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_tangerine_lots_start_price_positive",
                    "\"start_price\" > 0");
                
                tableBuilder.HasCheckConstraint(
                    "ck_tangerine_lots_current_price_valid",
                    "\"current_price\" >= \"start_price\"");
                
                tableBuilder.HasCheckConstraint(
                    "ck_tangerine_lots_buyout_price_valid",
                    "\"buyout_price\" IS NULL OR \"buyout_price\" >= \"start_price\"");
                
                tableBuilder.HasCheckConstraint(
                    "ck_tangerine_lots_auction_window_valid",
                    "\"auction_end_at\" > \"auction_start_at\"");
                
                tableBuilder.HasCheckConstraint(
                    "ck_tangerine_lots_expiration_valid",
                    "\"expiration_at\" >= \"auction_end_at\"");
                
                tableBuilder.HasCheckConstraint(
                    "ck_tangerine_lots_purchase_consistency",
                    "(\"purchase_type\" IS NULL AND \"buyer_id\" IS NULL) OR (\"purchase_type\" IS NOT NULL AND \"buyer_id\" IS NOT NULL)");
            });

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(4000);

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(x => x.StartPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.CurrentPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.BuyoutPrice)
                .HasPrecision(18, 2);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(x => x.Version)
                .IsRowVersion();

            builder.HasMany(x => x.Bids)
                .WithOne(x => x.Lot)
                .HasForeignKey(x => x.LotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.CurrentLeaderUserId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.BuyerId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ExpirationAt);
            builder.HasIndex(x => new { x.IsDeleted, x.ExpirationAt });
        }
    }
}
