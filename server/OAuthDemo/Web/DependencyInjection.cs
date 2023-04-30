using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using OAuthDemo.Application.Identity;
using OAuthDemo.Domain.Identity;
using OAuthDemo.Infrastructure.Persistence;
using OAuthDemo.Web.OAuthProvider;

namespace OAuthDemo.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services
            .AddAuth()
            .AddCors(ConfigureCors)
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
            .AddCookie(IdentityConstants.ApplicationScheme, ConfigureCookie)
            .AddCookie(IdentityConstants.ExternalScheme, ConfigureCookie)
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme)
            .AddOAuth("Github", ConfigureGithub);
        
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static void ConfigureGithub(OAuthOptions options)
    {
        options.SignInScheme = IdentityConstants.ApplicationScheme;
        options.ClientId = "78f484d7d1f631138c7e";
        options.ClientSecret = "a85be6b4ca79729e6a0d21198aab46f8cad57c4e";
        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
        options.UserInformationEndpoint = "https://api.github.com/user";
        options.CallbackPath = "/oauth/github-cb";
        options.SaveTokens = true;
        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        options.Scope.Add("user:email");
        options.Events.OnCreatingTicket = GithubOAuthProvider.OnCreatingTicket;
    }

    private static void ConfigureCookie(CookieAuthenticationOptions options)
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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
            var userName = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            //https://stackoverflow.com/questions/30946453/add-claim-on-successful-login
            return CurrentUserContext.Create(userManager, userName);
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

    private static void ConfigureCors(CorsOptions opt)
    {
        opt.AddPolicy("CorsPolicy", policy =>
        {
            policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders(
                    "WWW-Authenticate") // expose header so client can understand when to log user out
                .WithOrigins("http://localhost:8080"); // required when access resource from a different domain
        });
    }
}