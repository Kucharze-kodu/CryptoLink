using Microsoft.EntityFrameworkCore;


namespace CryptoLink.Architecture.Database
{
    public class CryptoLinkDbContext : DbContext
    {
        CryptoLinkDbContext(DbContextOptions<CryptoLinkDbContext> options)
        : base(options) 
        {
        
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(CryptoLinkDbContext).Assembly);
            base.OnModelCreating(modelBuilder);



        }
    }
}
