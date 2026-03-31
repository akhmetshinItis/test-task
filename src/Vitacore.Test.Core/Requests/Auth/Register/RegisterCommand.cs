using MediatR;
using Vitacore.Test.Contracts.Requests.Auth.Register;

namespace Vitacore.Test.Core.Requests.Auth.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public RegisterCommand(RegisterRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public RegisterRequest Request { get; }
    }
}
