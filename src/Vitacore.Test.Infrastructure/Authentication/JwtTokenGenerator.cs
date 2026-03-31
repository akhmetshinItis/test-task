using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Vitacore.Test.Data.Postgres.Identity;
using Vitacore.Test.Infrastructure.Authentication.Options;

namespace Vitacore.Test.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public JwtTokenResult GenerateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var rolesArray = roles.ToArray();
            var token = GenerateToken(user, rolesArray, out var expiresAtUtc);

            return new JwtTokenResult
            {
                AccessToken = token,
                ExpiresAtUtc = expiresAtUtc
            };
        }

        private string GenerateToken(ApplicationUser user, IReadOnlyCollection<string> roles, out DateTime expiresAtUtc)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
