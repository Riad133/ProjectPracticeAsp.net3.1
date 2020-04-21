using Microsoft.Extensions.DependencyInjection;
using Utility.Helper;

namespace Utility
{
    public class UtilityDependency
    {
        public static void AllDependency(IServiceCollection services)
        {
            services.AddSingleton(typeof(TaposRSA));
        }
    }
}