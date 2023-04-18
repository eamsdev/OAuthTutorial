using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OAuthDemo.Domain.Identity;

namespace OAuthDemo.Application.Identity.Features;

public class LoginUser
{
    public class InputModel : IRequest<ResponseModel>
    {
        [EmailAddress] 
        public required string Email { get; set; } = string.Empty;

        public required string Password { get; set; } = string.Empty;
    }

    public class ResponseModel
    { }

    public class Handler : IRequestHandler<InputModel, ResponseModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public Handler(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public async Task<ResponseModel> Handle(InputModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new AuthenticationException();
            }

            await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            
            return new ResponseModel();
        }
    }
}