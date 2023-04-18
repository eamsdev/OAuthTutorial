using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthDemo.Application.Identity;
using OAuthDemo.Domain.Identity;
using OAuthDemo.Infrastructure.Persistence;

namespace OAuthDemo.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.Events.OnRedirectToLogin = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.FromResult<object>(null!);
                };
            });
        services.AddAuthorization();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();
        
        services.AddProblemDetails(opt =>
        {
            opt.MapToStatusCode<AuthenticationException>(StatusCodes.Status401Unauthorized);
            opt.IncludeExceptionDetails = (context, exception) => false;
        }).AddControllers().AddProblemDetailsConventions()
            .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        
        services.AddScoped(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var userManager = sp.GetRequiredService<UserManager<User>>();
            var userEmail = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            return CurrentUserContext.Create(userManager, userEmail);
        });
        
        services.AddControllers();
        services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen();
        services.ConfigureSwaggerGen(options =>
        {
            options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
        });
        return services;
    }
}