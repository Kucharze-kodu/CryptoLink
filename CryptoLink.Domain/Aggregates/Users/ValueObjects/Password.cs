using CryptoLink.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace CryptoLink.Domain.Aggregates.Users.ValueObjects;

public sealed class Password : ValueObject
{
    public string Value { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }

    [JsonConstructor]
    private Password(string value)
    {
        Value = value;
        LastModifiedOnUtc = DateTime.UtcNow;
    }

    public static Password Create(string value) => new Password(value);

    public Password Load(
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