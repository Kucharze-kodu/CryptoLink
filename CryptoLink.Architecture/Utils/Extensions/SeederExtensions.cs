using CryptoLink.Architecture.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CryptoLink.Architecture.Database.Seeders;

namespace CryptoLink.Architecture.Utils.Extensions
{
    public static class SeederExtensions
    {
        public async static void ApplySeeder(this WebApplication app)
        {
            using var scope = app
                .Services.
                CreateScope();
            var dbContext = scope
            .ServiceProvider
            .GetRequiredService<CryptoLinkDbContext>();

            await DatabaseSeeder.SeedAsync(dbContext);
        }
    }
}
