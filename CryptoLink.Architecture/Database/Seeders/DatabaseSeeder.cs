using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Architecture.Database.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(CryptoLinkDbContext context)
        {
            await BookWordSeeder.SeedAsync(context);
        }

    }
}
