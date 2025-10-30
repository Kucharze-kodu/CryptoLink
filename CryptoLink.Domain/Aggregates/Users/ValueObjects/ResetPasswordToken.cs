using CryptoLink.Domain.Aggregates.Users.Enums;
using CryptoLink.Domain.Common.Interfaces;
using CryptoLink.Domain.Common.Primitives;
using Newtonsoft.Json;

namespace CryptoLink.Domain.Aggregates.Users.ValueObjects;

public sealed class ResetPasswordToken : ValueObject, IToken
{
    public Guid Value { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public DateTime LastSendOnUtc { get; private set; }
    public DateTime? UsedOnUtc { get; private set; }
    public TokenType Type { get; private set; }
    
    private const int MinimumTimeToResendInMinutes = 5;
    private const int TokenValidityInDays = 1;

    [JsonConstructor]
    private ResetPasswordToken()
    {
        Value = Guid.NewGuid();
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = DateTime.UtcNow.AddDays(TokenValidityInDays);
        LastSendOnUtc = DateTime.UtcNow;
        Type = TokenType.ResetPasswordToken;
    }

    public static ResetPasswordToken Create() => new ResetPasswordToken();

    public ResetPasswordToken Load(
        Guid value,
        DateTime createdOnUtc,
        DateTime expiresOnUtc,
        DateTime lastSendOnUtc,
        DateTime? usedOnUtc,
        TokenType type
    )
    {
        Value = value;
        CreatedOnUtc = createdOnUtc;
        ExpiresOnUtc = expiresOnUtc;
        LastSendOnUtc = lastSendOnUtc;
        UsedOnUtc = usedOnUtc;
        Type = type;
        return this;
    }
    
    public bool Verify(Guid token)
    {
        // Check if the token is valid
        if (Value != token)
        {
            return false;
        }
        
        // Check if the token is expired
        if (IsTokenExpired())
        {
            return false;
        }

        UsedOnUtc = DateTime.UtcNow;
        return true;
    }
    
    public TokenStatus CheckStatus()
    {
        // Check if the token was already sent
        if (IsTokenAlreadySent())
        {
            return TokenStatus.TokenNotReadyToResend;
        }
        
        // Check if the token is expired
        if (IsTokenExpired())
        {
            return TokenStatus.TokenExpired;
        }
        
        // Check if the token was already used
        if (UsedOnUtc is not null)
        {
            return TokenStatus.TokenUsed;
        }

        return TokenStatus.TokenReadyToResend;
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return CreatedOnUtc;
        yield return ExpiresOnUtc;
        yield return LastSendOnUtc;
        yield return UsedOnUtc;
        yield return Type;
    }
    
    private bool IsTokenAlreadySent()
    {
        return LastSendOnUtc
            .AddMinutes(MinimumTimeToResendInMinutes)
            .CompareTo(DateTime.UtcNow) != -1;
    }
    
    private bool IsTokenExpired()
    {
        return ExpiresOnUtc
            .CompareTo(DateTime.UtcNow) != 1;
    }
}