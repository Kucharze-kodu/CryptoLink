using CryptoLink.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace CryptoLink.Domain.Aggregates.Users.ValueObjects;

public sealed class PublicKey : ValueObject
{
    public string Value { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }

    [JsonConstructor]
    private PublicKey(string value)
    {
        Value = value;
        LastModifiedOnUtc = DateTime.UtcNow;
    }

    public static PublicKey Create(string value) => new PublicKey(value);

    public PublicKey Load(
        string value,
        DateTime lastModifiedOnUtc
    )
    {
        Value = value;
        LastModifiedOnUtc = lastModifiedOnUtc;
        return this;
    }
    
    public bool IsExpired(int days) => LastModifiedOnUtc.AddDays(days) < DateTime.UtcNow;
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return LastModifiedOnUtc;
    }
}