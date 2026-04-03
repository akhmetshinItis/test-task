using Microsoft.Extensions.DependencyInjection;
using Vitacore.Test.Core.Interfaces.Lots;
using Vitacore.Test.Core.Services.Lots;

namespace Vitacore.Test.Core
{
    public static class Entry
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(Entry)));
            services.AddScoped<ITangerineLotBackgroundGenerator, TangerineLotBackgroundGenerator>();
            
            return services;
        }
    }
}
