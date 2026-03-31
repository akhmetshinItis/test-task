using Vitacore.Test.Contracts.Requests.Auth.Login;
using Vitacore.Test.Contracts.Requests.Auth.Register;
using Vitacore.Test.Core.Models.Auth;

namespace Vitacore.Test.Core.Interfaces.Auth
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<AuthenticationResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    }
}
