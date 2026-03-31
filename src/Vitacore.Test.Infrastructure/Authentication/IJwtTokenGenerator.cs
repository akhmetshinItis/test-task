using Vitacore.Test.Data.Postgres.Identity;

namespace Vitacore.Test.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        JwtTokenResult GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
