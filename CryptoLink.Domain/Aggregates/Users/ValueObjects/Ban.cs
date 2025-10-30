using CryptoLink.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace CryptoLink.Domain.Aggregates.Users.ValueObjects;

public sealed class Ban : ValueObject
{
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ExpiresOnUtc { get; private set; }
    public string Message { get; private set; }

    [JsonConstructor]
    private Ban(string message = "Ban", DateTime? expiresOnUtc = null)
    {
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = expiresOnUtc ?? DateTime.UtcNow.AddDays(1);
        Message = message;
    }

    public static Ban Create(
        string? message = "Ban", 
        DateTime? expiresOnUtc = null) 
        => new Ban(message, expiresOnUtc);

    public Ban Load(
        DateTime createdOnUtc,
        DateTime? expiresOnUtc,
        string message
    )
    {
        CreatedOnUtc = createdOnUtc;
        ExpiresOnUtc = expiresOnUtc;
        Message = message;
        return this;
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return CreatedOnUtc;
        yield return ExpiresOnUtc;
        yield return Message;
    }
}