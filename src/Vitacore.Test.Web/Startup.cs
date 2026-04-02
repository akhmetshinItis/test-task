using Hangfire;
using Hangfire.PostgreSql;
using Vitacore.Test.Core;
using Vitacore.Test.Core.Options;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Infrastructure;
using Vitacore.Test.Infrastructure.Email.Options;
using Vitacore.Test.Web.Configuration;

namespace Vitacore.Test.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly HangfireOptions _hangfireOptions;
        
        public Startup(IConfiguration configuration, HangfireOptions hangfireOptions)
        {
            _configuration = configuration;
            _hangfireOptions = hangfireOptions ?? throw new ArgumentNullException(nameof(hangfireOptions));
        }
        
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var tangerineLotGenerationOptions = _configuration
                .GetSection(TangerineLotGenerationOptions.SectionName)
                .Get<TangerineLotGenerationOptions>()
                ?? new TangerineLotGenerationOptions();
            var emailOptions = BuildEmailOptions();

            services.AddSingleton(tangerineLotGenerationOptions);
            services.AddSingleton(emailOptions);

            return services
                .AddCore()
                .AddPostgres(_configuration["Application:DbConnectionString"] ?? throw new ArgumentNullException())
                .AddInfrastructure(_configuration);
        }

        public IServiceCollection ConfigureHangfire(IServiceCollection services)
        {
            if (string.IsNullOrWhiteSpace(_hangfireOptions.ConnectionString))
            {
                throw new InvalidOperationException("Hangfire connection string is required.");
            }

            if (_hangfireOptions.EnableDashboard && string.IsNullOrWhiteSpace(_hangfireOptions.DashboardPath))
            {
                throw new InvalidOperationException("Hangfire dashboard path is required.");
            }

            services.AddHangfire((_, config) =>
            {
                var storageOptions = new PostgreSqlStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    QueuePollInterval = TimeSpan.FromSeconds(15)
                };

                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(
                        options => options.UseNpgsqlConnection(_hangfireOptions.ConnectionString),
                        storageOptions);
            });

            services.AddHangfireServer(options =>
            {
                if (_hangfireOptions.WorkerCount > 0)
                {
                    options.WorkerCount = _hangfireOptions.WorkerCount;
                }
            });

            return services;
        }

        private EmailOptions BuildEmailOptions()
        {
            var section = _configuration.GetSection(EmailOptions.SectionName);

            return new EmailOptions
            {
                Host = section["Host"] ?? throw new ArgumentNullException("Email:Host"),
                Port = int.TryParse(section["Port"], out var port)
                    ? port
                    : throw new ArgumentNullException("Email:Port"),
                EnableSsl = bool.TryParse(section["EnableSsl"], out var enableSsl) && enableSsl,
                UseDefaultCredentials = bool.TryParse(section["UseDefaultCredentials"], out var useDefaultCredentials) && useDefaultCredentials,
                UserName = section["UserName"],
                Password = section["Password"],
                FromAddress = section["FromAddress"] ?? throw new ArgumentNullException("Email:FromAddress"),
                FromName = section["FromName"]
            };
        }
    }
}
