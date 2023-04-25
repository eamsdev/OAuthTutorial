using System.Net;
using FluentAssertions;
using OAuthDemo.Application.Identity.Features;
using OAuthDemo.Tests.Extensions;

namespace OAuthDemo.Tests;

public class IdentityTests
{
    [Fact]
    public async Task CanRegisterUser()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        
        // When
        var response = await RegisterAndLoginUser(client);

        // Then
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task CannotLogoutIfNotLoggedIn()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        
        // When
        var response = await client.PostRouteAsJsonAsync("identity/logout", new LogoutUser.InputModel());

        // Then
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task CanLogout()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        await RegisterAndLoginUser(client);
        
        // When
        var response = await Logout(client);

        // Then
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task CanGetCurrentUserIfLoggedIn()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        await RegisterAndLoginUser(client);
        
        // When
        var response = await client.GetAsync("identity/user");

        // Then
        response.IsSuccessStatusCode.Should().BeTrue();
    }
    
    [Fact]
    public async Task CannotGetCurrentUserIfNotLoggedIn()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        
        // When
        var response = await client.GetAsync("identity/user");

        // Then
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task CannotGetCurrentUserAfterLoggedOut()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        await RegisterAndLoginUser(client);
        await Logout(client);
        
        // When
        var response = await client.GetAsync("identity/user");

        // Then
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task CanRegisterThenLogoutThenLoginThenAccessProtectedEndpoint()
    {
        // Given
        await using var webAppFactory = new CustomWebApplicationFactory(); 
        using var client = webAppFactory.CreateClient();
        var user = await RegisterAndGetUser(client);
        await Logout(client);
        var loginInput = new LoginUser.InputModel
        {
            Username = user.Username,
            Password = user.Password
        };

        // When
        var response = await client.PostRouteAsJsonAsync("identity/login", loginInput);

        // Then
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    private static async Task<HttpResponseMessage> Logout(HttpClient client)
    {
        return await client.PostRouteAsJsonAsync("identity/logout", new LogoutUser.InputModel());
    }
    
    private static async Task<HttpResponseMessage> RegisterAndLoginUser(HttpClient client)
    {
        var registerRequest = new RegisterUser.InputModel
        {
            Username = Faker.StringFaker.Alpha(8),
            Password = "Password_1234!!"
        };
        
        return await client.PostRouteAsJsonAsync("identity/register", registerRequest);
    }
    
    private static async Task<RegisterUser.InputModel> RegisterAndGetUser(HttpClient client)
    {
        var registerRequest = new RegisterUser.InputModel
        {
            Username = Faker.StringFaker.Alpha(8),
            Password = "Password_1234!!"
        };
        
        await client.PostRouteAsJsonAsync("identity/register", registerRequest);
        return registerRequest;
    }
}