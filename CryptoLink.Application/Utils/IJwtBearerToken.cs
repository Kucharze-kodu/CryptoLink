namespace CryptoLink.Application.Utils;

public interface IJwtBearerToken
{
    public string Token { get; init; }
    public DateTime ExpiresOnUtc { get; init; }
}