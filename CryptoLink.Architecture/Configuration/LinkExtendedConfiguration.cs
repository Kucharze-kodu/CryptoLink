using CryptoLink.Domain.Aggregates.LinkExtendeds;
// Upewnij się, że ta ścieżka jest poprawna, jeśli zmieniłeś literówkę w folderach
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoLink.Architecture.Configuration
{
    public sealed class LinkExtendedConfiguration : IEntityTypeConfiguration<LinkExtended>
    {
        public void Configure(EntityTypeBuilder<LinkExtended> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasConversion(
                    c => c.Value,
                    value => LinkExtendedId.Create(value))
                .ValueGeneratedOnAdd();

            builder.Property(c => c.UserId)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value)
                );
        }
    }
}