using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CryptoLink.Architecture.Configuration
{
    public sealed class ConfigurationLinkExtended : IEntityTypeConfiguration<LinkExtended>
    {
        public void Configure(EntityTypeBuilder<LinkExtended> builder)
        {
            // Id
            builder
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Id)
                .HasConversion(
                    c => c.Value,
                    value => LinkExtendedId.Create(value))
                .ValueGeneratedOnAdd();


            // Counter link
            builder
                .OwnsOne(r => r.Links, links =>
                {
                    links.ToTable("Links");
                    links.Property(x => x.Count).HasColumnName("Count");
                });
        }


    }
}
