using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Vitacore.Test.Data.Postgres
{
    public static class Entry
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException(nameof(connectionString));
            
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
                opt.UseSnakeCaseNamingConvention();
            });
            
            return services;
        }
    }
}