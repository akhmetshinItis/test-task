using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitacore.Test.Core.Entities;

namespace Vitacore.Test.Data.Postgres.Configurations
{
    public class OutboxMessageConfiguration : BaseEntityConfiguration<OutboxMessage>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasMaxLength(1024)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.Payload)
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.RetryCount)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.NextAttemptAt)
                .IsRequired();

            builder.HasIndex(x => x.NextAttemptAt);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
