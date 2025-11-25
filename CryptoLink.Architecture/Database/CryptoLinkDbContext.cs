using CryptoLink.Domain.Aggregates.BookWords;
using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.Users;
using Microsoft.EntityFrameworkCore;


namespace CryptoLink.Architecture.Database
{
    public class CryptoLinkDbContext : DbContext
    {
        public CryptoLinkDbContext(DbContextOptions<CryptoLinkDbContext> options)
        : base(options) 
        {
        
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BookWord> BookWords{ get; set;}
        public DbSet<LinkExtended> LinkExtendeds{ get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(CryptoLinkDbContext).Assembly);
            base.OnModelCreating(modelBuilder);




        }
    }
}
