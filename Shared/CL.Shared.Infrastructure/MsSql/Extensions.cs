using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CL.Shared.Infrastructure.MsSql;

public static class Extensions
{
    public static IServiceCollection AddMSSql<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var options = configuration.GetOptions<MsSqlOptions>("MsSql");

        services.AddDbContext<T>((provider, opt) =>
        {
            opt.UseSqlServer(options.ConnectionString);
            opt.UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>());
            //opt.AddInterceptors(provider.GetRequiredService<AggregateRootInterceptor>());
        });

        return services;
    }

    public static IApplicationBuilder MigrateContext<T>(this IApplicationBuilder app) where T : DbContext
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var service = serviceScope.ServiceProvider.GetRequiredService<T>();
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<T>>();

            logger.LogInformation("Migrating context {Context}", typeof(T).Name);

            service.Database.OpenConnection();

            if (service.Database.CanConnect())
            {
                service.Database.Migrate();
                logger.LogInformation("Context: {Context} Migrated", typeof(T).Name);
            }
            else
            {
                throw new Exception($"Cannot connect to database: {typeof(T).Name}");
            }

            return app;
        }
    }
}