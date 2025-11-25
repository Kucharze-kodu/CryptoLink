using CryptoLink.Application.Utils;

namespace CryptoLink.Architecture.Authentication;

public record JwtBearerToken(
    string Token,
    DateTime ExpiresOnUtc) : IJwtBearerToken;