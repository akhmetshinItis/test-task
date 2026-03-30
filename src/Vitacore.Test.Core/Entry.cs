using Microsoft.Extensions.DependencyInjection;

namespace Vitacore.Test.Core
{
    public static class Entry
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(Entry)));
            
            return services;
        }
    }
}