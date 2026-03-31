using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vitacore.Test.Core.Authorization;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Data.Postgres.Identity;
using Vitacore.Test.Infrastructure.Authentication.Options;

namespace Vitacore.Test.Infrastructure.Authentication
{
    public class IdentitySeeder
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AdminUserOptions _adminUserOptions;

        public IdentitySeeder(
            AppDbContext dbContext,
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager,
            AdminUserOptions adminUserOptions)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _adminUserOptions = adminUserOptions;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            await EnsureRoleExistsAsync(ApplicationRoles.Admin);
            await EnsureRoleExistsAsync(ApplicationRoles.User);
            await EnsureAdminExistsAsync();
        }

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return;
            }

            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
            }
        }

        private async Task EnsureAdminExistsAsync()
        {
            var admin = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == _adminUserOptions.Email);

            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = _adminUserOptions.UserName,
                    Email = _adminUserOptions.Email,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(admin, _adminUserOptions.Password);

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", createResult.Errors.Select(x => x.Description)));
                }
            }

            await EnsureUserInRoleAsync(admin, ApplicationRoles.Admin);
            await EnsureUserInRoleAsync(admin, ApplicationRoles.User);
        }

        private async Task EnsureUserInRoleAsync(ApplicationUser user, string roleName)
        {
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
            }
        }
    }
}
