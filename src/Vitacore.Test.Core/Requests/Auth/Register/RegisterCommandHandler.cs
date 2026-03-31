using MediatR;
using Vitacore.Test.Contracts.Requests.Auth.Register;
using Vitacore.Test.Core.Interfaces.Auth;

namespace Vitacore.Test.Core.Requests.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(request.Request, cancellationToken);

            return new RegisterResponse
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
