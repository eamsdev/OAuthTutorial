using Microsoft.AspNetCore.Identity;

namespace OAuthDemo.Domain.Identity;

public class User : IdentityUser
{
    public required IdentityProvider IdentityProvider { get; set; }
}