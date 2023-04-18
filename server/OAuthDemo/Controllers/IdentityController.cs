using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OAuthDemo.Application.Identity.Features;

namespace OAuthDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(
        ILogger<IdentityController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
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
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUser.InputModel inputModel)
    {
        await _mediator.Send(inputModel);
        return Ok();
    }
}