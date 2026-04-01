using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vitacore.Test.Core;
using Vitacore.Test.Core.Interfaces.Auth;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Data.Postgres.Identity;
using Vitacore.Test.Infrastructure.Background.Jobs;
using Vitacore.Test.Infrastructure.Authentication;
using Vitacore.Test.Infrastructure.Authentication.Options;
using Vitacore.Test.Infrastructure.Lots;

namespace Vitacore.Test.Infrastructure
{
    public static class Entry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = BuildJwtOptions(configuration);
            var adminUserOptions = BuildAdminUserOptions(configuration);

            services.AddSingleton(jwtOptions);
            services.AddSingleton(adminUserOptions);

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();
            services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetLotsQueryHandler>());

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAppDbContext>(x => x.GetRequiredService<AppDbContext>());
            services.AddScoped<CompleteEndedAuctionsJob>();
            services.AddScoped<CleanupExpiredLotsJob>();
            services.AddScoped<IdentitySeeder>();

            return services;
        }

        public static async Task SeedIdentityAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
            await seeder.SeedAsync();
        }

        private static JwtOptions BuildJwtOptions(IConfiguration configuration)
        {
            var section = configuration.GetSection(JwtOptions.SectionName);

            return new JwtOptions
            {
                Issuer = section["Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer"),
                Audience = section["Audience"] ?? throw new ArgumentNullException("Jwt:Audience"),
                SigningKey = section["SigningKey"] ?? throw new ArgumentNullException("Jwt:SigningKey"),
                AccessTokenLifetimeMinutes = int.TryParse(section["AccessTokenLifetimeMinutes"], out var lifetime)
                    ? lifetime
                    : throw new ArgumentNullException("Jwt:AccessTokenLifetimeMinutes")
            };
        }

        private static AdminUserOptions BuildAdminUserOptions(IConfiguration configuration)
        {
            var section = configuration.GetSection(AdminUserOptions.SectionName);

            return new AdminUserOptions
            {
                UserName = section["UserName"] ?? throw new ArgumentNullException("AdminUser:UserName"),
                Email = section["Email"] ?? throw new ArgumentNullException("AdminUser:Email"),
                Password = section["Password"] ?? throw new ArgumentNullException("AdminUser:Password")
            };
        }
    }
}
