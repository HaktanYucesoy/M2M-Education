using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Smartup.M2M.BuildingBlocks.Security;

namespace Smartup.M2M.BuildingBlocks.Extensions
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddM2MAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(M2MAuthenticationOptions.DefaultScheme)
                    .AddScheme<M2MAuthenticationOptions, M2MAuthenticationHandler>(
                        M2MAuthenticationOptions.DefaultScheme,
                        options => configuration.GetSection("M2MAuth").Bind(options));

            return services.AddAuthorization();
        }
    }
}
