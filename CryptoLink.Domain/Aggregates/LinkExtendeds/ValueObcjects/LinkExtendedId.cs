using CryptoLink.Domain.Common.Primitives;
using System.Text.Json.Serialization;


namespace CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects
{
    public sealed class LinkExtendedId : ValueObject
    {
        public int Value { get; }

        [JsonConstructor]
        private LinkExtendedId(int value)
        {
            Value = value;
        }
        public static LinkExtendedId Create(int id) => new LinkExtendedId(id);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
