using Vitacore.Test.Contracts.Requests.Auth.Login;
using MediatR;

namespace Vitacore.Test.Core.Requests.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}