using MediatR;
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
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
    
    [HttpGet("who")]
    public async Task<IActionResult> WhoAmI()
    {
        return await Task.FromResult(Ok(_currentUserContext.User()));
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
}