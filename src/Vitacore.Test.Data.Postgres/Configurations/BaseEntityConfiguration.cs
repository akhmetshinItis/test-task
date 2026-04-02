using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitacore.Test.Core.Abstractions;

namespace Vitacore.Test.Data.Postgres.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                builder.Property<bool>(nameof(ISoftDeletable.IsDeleted))
                    .IsRequired()
                    .HasDefaultValue(false);

                builder.Property<DateTime?>(nameof(ISoftDeletable.DeletedAt));
            }

            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
