using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoApi.Persistence.Configurations;

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedBy).HasMaxLength(150).IsRequired();
        builder.Property(e => e.CreatedOn);
        builder.Property(e => e.LastModifiedBy).HasMaxLength(150).IsRequired();
        builder.Property(e => e.LastModifiedOn);
    }
}
