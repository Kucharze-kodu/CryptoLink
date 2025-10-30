using CryptoLink.Architecture.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoLink.Architecture
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CryptoLinkDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    r =>
                        r.MigrationsAssembly(typeof(DependencyInjection).Assembly.ToString())));





            return services;
        }


     }
}
