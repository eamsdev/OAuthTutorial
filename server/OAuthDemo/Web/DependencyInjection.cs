using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthDemo.Application.Identity;
using OAuthDemo.Domain.Identity;
using OAuthDemo.Infrastructure.Persistence;

namespace OAuthDemo.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services
            .AddAuth()
            .AddProblemDetails()
            .AddUserContext()
            .AddControllerInternal()
            .AddApiDoc();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            // AddIdentityCore(...) doesnt add these by default
            .AddCookie(IdentityConstants.ExternalScheme)
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, ConfigureCookie);
        
        services.AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static void ConfigureCookie(CookieAuthenticationOptions options)
    {
        options.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult<object>(null!);
        };
    }

    private static IServiceCollection AddProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(opt =>
        {
            opt.MapToStatusCode<AuthenticationException>(StatusCodes.Status401Unauthorized);
            opt.IncludeExceptionDetails = (context, exception) => false;
        });
        
        return services;
    }
    
    private static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddScoped(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var userManager = sp.GetRequiredService<UserManager<User>>();
            var userEmail = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            return CurrentUserContext.Create(userManager, userEmail);
        });
        
        return services;
    }

    private static IServiceCollection AddControllerInternal(this IServiceCollection services)
    {

        services.AddControllers()
            .AddProblemDetailsConventions()
            .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        
        return services;
    }
    
    private static IServiceCollection AddApiDoc(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
            });
        
        return services;
    }
}