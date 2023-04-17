using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthDemo.Domain.Identity;
using OAuthDemo.Infrastructure.Persistence;

namespace OAuthDemo.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication("Cookie").AddCookie("Cookie");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });
        services.AddIdentityCore<User>(
            options =>
            {
                options.User.RequireUniqueEmail = true;
            }
        ).AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager<SignInManager<User>>();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen();
        
        return services;
    }
}