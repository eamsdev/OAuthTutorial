using Microsoft.EntityFrameworkCore;
using OAuthDemo.Infrastructure.Persistence;

namespace OAuthDemo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDb(config);
        return services;
    }
    
    private static IServiceCollection AddDb(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}