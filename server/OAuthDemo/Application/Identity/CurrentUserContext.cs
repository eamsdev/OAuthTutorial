using Microsoft.AspNetCore.Identity;
using OAuthDemo.Domain.Identity;

namespace OAuthDemo.Application.Identity;

public class CurrentUserContext
{
    private readonly Lazy<Task<User?>> _lazyGetUserTask;
    
    private CurrentUserContext(Lazy<Task<User?>> lazyGetUserTask)
    {
        _lazyGetUserTask = lazyGetUserTask;
    }

    public async Task<User?> TryGetUser() => await _lazyGetUserTask.Value;

    public async Task<User> User()
    {
        var user = await _lazyGetUserTask.Value;
        if (user is null)
        {
            throw new InvalidOperationException("User is not valid or does not exist.");
        }

        return user;
    }

    public static CurrentUserContext Create(
        UserManager<User> userManager, 
        string? userEmail)
    {
        return new CurrentUserContext(
            new Lazy<Task<User?>>(async () => 
                userEmail is null ? null : await userManager.FindByEmailAsync(userEmail)));
    }
}