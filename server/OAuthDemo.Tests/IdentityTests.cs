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
        var response = await LoginUser(client);

        // Then
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task CantLogoutIfNotLoggedIn()
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
        await LoginUser(client);
        
        // When
        var response = await client.PostRouteAsJsonAsync("identity/logout", new LogoutUser.InputModel());

        // Then
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    private static async Task<HttpResponseMessage> LoginUser(HttpClient client)
    {
        var registerRequest = new RegisterUser.InputModel
        {
            Email = Faker.InternetFaker.Email(),
            Password = "Password_1234!!"
        };
        
        return await client.PostRouteAsJsonAsync("identity/register", registerRequest);
    }
}