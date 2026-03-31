using Vitacore.Test.Core;
using Vitacore.Test.Data.Postgres;
using Vitacore.Test.Infrastructure;

namespace Vitacore.Test.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IServiceCollection ConfigureServices(IServiceCollection services)
            => services
                .AddCore()
                .AddPostgres(_configuration["Application:DbConnectionString"] ?? throw new ArgumentNullException())
                .AddInfrastructure(_configuration);
    }
}
