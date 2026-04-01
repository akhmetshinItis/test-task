using Hangfire;
using Hangfire.PostgreSql;
using Vitacore.Test.Core;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Infrastructure;
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
            => services
                .AddCore()
                .AddPostgres(_configuration["Application:DbConnectionString"] ?? throw new ArgumentNullException())
                .AddInfrastructure(_configuration);

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
    }
}
