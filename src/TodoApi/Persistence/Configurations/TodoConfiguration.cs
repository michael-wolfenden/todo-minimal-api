using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Todos;

namespace TodoApi.Persistence.Configurations;

public class TodoConfiguration : BaseConfiguration<Todo>
{
    public override void Configure(EntityTypeBuilder<Todo> builder)
    {
        base.Configure(builder);

        builder.ToTable("Todo");
        builder.Property(t => t.Title).HasMaxLength(200).IsRequired();
        builder.Property(t => t.IsCompleted);
    }
}
