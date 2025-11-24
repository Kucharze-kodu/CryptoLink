
using CryptoLink.Domain.Aggregates.LinkExtended;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CryptoLink.Architecture.Configuration
{
    public sealed class ConfigurationLinkExtended : IEntityTypeConfiguration<LinkExtended>
    {



        public void Configure(EntityTypeBuilder<LinkExtended> builder)
        {



            // Counter link
            builder
                .OwnsOne(r => r.Links, links =>
                {
                    links.ToTable("Bans");
                    links.Property(x => x.Count).HasColumnName("Count");
                });
        }


    }
}
