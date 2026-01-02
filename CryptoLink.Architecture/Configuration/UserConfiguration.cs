using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.Users;
using CryptoLink.Domain.Aggregates.Users.Enums;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CryptoLink.Architecture.Configuration
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {


        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Id
            builder
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Id)
                .HasConversion(
                    c => c.Value,
                    value => UserId.Create(value))
                .ValueGeneratedOnAdd();

            // Name
            builder
                .Property(r => r.Name)
                .HasMaxLength(255);

            builder
                .HasIndex(r => r.Name)
                .IsUnique();

            // Ban
            builder
                .OwnsOne(r => r.Ban, ban =>
                {
                    ban.ToTable("Bans");
                    ban.Property(x => x.CreatedOnUtc).HasColumnName("CreatedOnUtc");
                    ban.Property(x => x.ExpiresOnUtc).HasColumnName("ExpiresOnUtc");
                    ban.Property(x => x.Message).HasColumnName("Message");
                });

            // Role
            builder
                .Property(r => r.Role)
                .HasConversion(
                    c => c.ToString(),
                    c => (Role)Enum.Parse(typeof(Role), c))
                .HasColumnName("Role");
        }
    }
}
