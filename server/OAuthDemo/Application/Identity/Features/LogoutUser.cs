using MediatR;
using Microsoft.AspNetCore.Identity;
using OAuthDemo.Domain.Identity;

namespace OAuthDemo.Application.Identity.Features;

public class LogoutUser
{
    public class InputModel : IRequest<ResponseModel>
    { }

    public class ResponseModel
    { }

    public class Handler : IRequestHandler<InputModel, ResponseModel>
    {
        private readonly SignInManager<User> _signInManager;

        public Handler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        
        public async Task<ResponseModel> Handle(InputModel request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return new ResponseModel();
        }
    }
}