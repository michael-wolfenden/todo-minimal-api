using Microsoft.EntityFrameworkCore;
using TodoApi.Persistence;
using TodoApi.Todos;

namespace TodoApi.Configure;

public static class Persistence
{
    public static WebApplicationBuilder AddCustomPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddCustomPersistence("name=ConnectionStrings:DefaultConnection");

        return builder;
    }

    public static void AddCustomPersistence(this IServiceCollection services, string connectionString)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TodoDbContext>));
        if (descriptor != null) services.Remove(descriptor);

        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                o =>
                {
                    o.EnableRetryOnFailure();
                    o.MigrationsAssembly(typeof(TodoDbContext).Assembly.FullName);
                }));
    }

    public static async Task EnsureDbCreated(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        await using var applicationDbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TodoDbContext>>();

        logger.LogInformation("Creating the database...");

        if (application.Environment.IsDevelopment())
        {
            logger.LogInformation("[Development mode] Dropping the database...");
            await applicationDbContext.Database.EnsureDeletedAsync();
        }

        await applicationDbContext.Database.MigrateAsync();

        if (application.Environment.IsDevelopment())
        {
            logger.LogInformation("[Development mode] Seeding the database...");
            if (applicationDbContext.Todos.Any()) return;

            var todos = new Todo[]
            {
                new("Lone Pine Koala Sanctuary"),
                new("South Bank Parklands"),
                new("Mount Coot-Tha Summit Lookout"),
                new("Brisbane Botanic Gardens Mt. Coot-tha"),
                new("City Cat"),
                new("XXXX Brewery Tour"),
                new("City Hopper"),
                new("Roma Street Parkland"),
                new("Eat Street Northshore"),
                new("City Botanic Gardens")
            };

            await applicationDbContext.Todos.AddRangeAsync(todos);
            await applicationDbContext.SaveChangesAsync();
        }

        logger.LogInformation("Database created successfully");
    }
}
