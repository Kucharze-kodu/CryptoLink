using CryptoLink.Domain.Common.Primitives;
using System.Text.Json.Serialization;


namespace CryptoLink.Domain.Aggregates.BookWords.ValueObcjets
{
    public sealed class BookWordId : ValueObject
    {
        public int Value { get; }

        [JsonConstructor]
        private BookWordId(int value)
        {
            Value = value;
        }
        public static BookWordId Create(int id) => new BookWordId(id);
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
