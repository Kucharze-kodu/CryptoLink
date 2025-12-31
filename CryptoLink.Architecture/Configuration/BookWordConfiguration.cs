using CryptoLink.Domain.Aggregates.BookWords;
using CryptoLink.Domain.Aggregates.BookWords.ValueObcjets;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Architecture.Configuration
{
    public sealed class BookWordConfiguration : IEntityTypeConfiguration<BookWord>
    {

        public void Configure(EntityTypeBuilder<BookWord> builder)
        {
            // Id
            builder
                .HasKey(r => r.Id);

            builder
                .Property(r => r.Id)
                .HasConversion(
                    c => c.Value,
                    value => BookWordId.Create(value))
                .ValueGeneratedOnAdd();



        }
    }
}
