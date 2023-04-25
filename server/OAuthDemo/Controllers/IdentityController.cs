using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OAuthDemo.Application.Identity;
using OAuthDemo.Application.Identity.Features;

namespace OAuthDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserContext _currentUserContext;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(
        ILogger<IdentityController> logger,
        IMediator mediator,
        CurrentUserContext currentUserContext)
    {
        _logger = logger;
        _mediator = mediator;
        _currentUserContext = currentUserContext;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("login-github")]
    public IResult Login()
    {
        return Results.Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "http://localhost:8080/"
            }, 
            authenticationSchemes: new List<string> { "Github" });
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
    
    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var user = await _currentUserContext.User();
        return Ok(user.UserName);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
}