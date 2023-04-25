using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using OAuthDemo.Domain.Identity;

namespace OAuthDemo.Web.OAuthProvider;

public static class GithubOAuthProvider
{
    public static async Task OnCreatingTicket(OAuthCreatingTicketContext ctx)
    {
        var username = await RetrieveUsername(ctx);
        using var userManager = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
        await SignIn(ctx, 
            await TryGetExistingUser(userManager, username) 
            ?? await CreateUser(userManager, username));
    }

    private static async Task<User> CreateUser(UserManager<User> userManager, string userName)
    {
        var newUser = new User
        {
            IdentityProvider = IdentityProvider.Github,
            UserName = userName,
        };
        var createdUserResult = await userManager.CreateAsync(newUser);

        if (!createdUserResult.Succeeded)
        {
            throw new AuthenticationException();
        }

        return newUser;
    }

    private static async Task<User?> TryGetExistingUser(UserManager<User> userManager, string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        return user?.IdentityProvider is not IdentityProvider.Github ? null : user;
    }
    
    private static async Task SignIn(OAuthCreatingTicketContext ctx, User user)
    {
        var signInManager = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
        await signInManager.SignInAsync(user, true);
    }

    private static async Task<string> RetrieveUsername(OAuthCreatingTicketContext ctx)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
        using var getClaimResult = await ctx.Backchannel.SendAsync(request);
        ctx.RunClaimActions(await getClaimResult.Content.ReadFromJsonAsync<JsonElement>());

        try
        {
            var idClaim = ctx.Identity!.FindFirst(ClaimTypes.NameIdentifier);
            var loginClaim = ctx.Identity!.FindFirst(ClaimTypes.Name);
            var nativeLoginUsername = GenerateUserName(idClaim!.Value, loginClaim!.Value);
            ctx.Identity.RemoveClaim(loginClaim);
            ctx.Identity.AddClaim(new Claim(ClaimTypes.Name, nativeLoginUsername));
            return nativeLoginUsername;
        }
        catch (Exception e)
        {
            throw new AuthenticationException();
        }
    }

    private static string GenerateUserName(string userId, string userLogin) 
        => $"{userLogin}-{userId}";
}