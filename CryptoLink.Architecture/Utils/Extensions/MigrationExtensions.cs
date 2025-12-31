using CryptoLink.Architecture.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoLink.Architecture.Utils.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app
            .Services.
            CreateScope();
        var dbContext = scope
            .ServiceProvider
            .GetRequiredService<CryptoLinkDbContext>();
        dbContext.Database.Migrate();
    }
}