using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OAuthDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(ILogger<IdentityController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        throw new NotImplementedException();
        return await Task.FromResult(Ok());
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        throw new NotImplementedException();
        return await Task.FromResult(Ok());
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
        return await Task.FromResult(Ok());
    }
}