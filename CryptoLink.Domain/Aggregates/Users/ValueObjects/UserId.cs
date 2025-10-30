using CryptoLink.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace CryptoLink.Domain.Aggregates.Users.ValueObjects;

public sealed class UserId : ValueObject
{
    public int Value { get; }

    [JsonConstructor]
    private UserId(int value)
    {
        Value = value;
    }

    public static UserId Create(int id) => new UserId(id);
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}