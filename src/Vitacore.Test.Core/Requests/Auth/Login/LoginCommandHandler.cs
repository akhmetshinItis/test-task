using Vitacore.Test.Contracts.Requests.Auth.Login;
using Vitacore.Test.Core.Interfaces.Auth;
using MediatR;

namespace Vitacore.Test.Core.Requests.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(request.Request, cancellationToken);

            return new LoginResponse
            {
                UserId = result.UserId,
                UserName = result.UserName,
                Email = result.Email,
                AccessToken = result.AccessToken,
                ExpiresAtUtc = result.ExpiresAtUtc,
                Roles = result.Roles.ToArray()
            };
        }
    }
}
