using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoApi.Infrastructure.Services;
using TodoApi.Todos;

namespace TodoApi.Persistence;

public class TodoDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public TodoDbContext(DbContextOptions<TodoDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var now = DateTime.UtcNow;
        var userId = _currentUserService.User;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UpdateCreationProperties(now, userId);
                    entry.Entity.UpdateModifiedProperties(now, userId);
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, userId);
                    break;

                case EntityState.Deleted:
                    entry.Entity.UpdateModifiedProperties(now, userId);
                    break;
            }
        }
    }
}
