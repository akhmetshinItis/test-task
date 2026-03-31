using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Contracts.Requests.Auth.Login;
using Vitacore.Test.Contracts.Requests.Auth.Register;
using Vitacore.Test.Core.Authorization;
using Vitacore.Test.Core.Exceptions;
using Vitacore.Test.Core.Interfaces.Auth;
using Vitacore.Test.Core.Models.Auth;
using Vitacore.Test.Data.Postgres.Identity;

namespace Vitacore.Test.Infrastructure.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public IdentityService(UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await FindByEmailAsync(request.Email, cancellationToken);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new AuthenticationException("Неверный email или пароль.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return BuildAuthenticationResult(user, roles);
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
            {
                throw new AppException("Пользователь с таким email уже существует.");
            }

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
            {
                throw new AppException("Пользователь с таким именем уже существует.");
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new AppException(string.Join("; ", result.Errors.Select(x => x.Description)));
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, ApplicationRoles.User);

            if (!addRoleResult.Succeeded)
            {
                throw new AppException(string.Join("; ", addRoleResult.Errors.Select(x => x.Description)));
            }

            var roles = await _userManager.GetRolesAsync(user);
            return BuildAuthenticationResult(user, roles);
        }

        private AuthenticationResult BuildAuthenticationResult(ApplicationUser user, IEnumerable<string> roles)
        {
            var rolesArray = roles.ToArray();
            var token = _jwtTokenGenerator.GenerateToken(user, rolesArray);

            return new AuthenticationResult
            {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                AccessToken = token.AccessToken,
                ExpiresAtUtc = token.ExpiresAtUtc,
                Roles = rolesArray
            };
        }

        private async Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var normalizedEmail = email.Trim();

            if (string.IsNullOrWhiteSpace(normalizedEmail))
            {
                return null;
            }

            return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
        }
    }
}
