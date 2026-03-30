using Vitacore.Test.Contracts.Requests.Auth.Login;
using MediatR;

namespace Vitacore.Test.Core.Requests.Auth.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public LoginCommand(LoginRequest request)
        {
        }
    }
}