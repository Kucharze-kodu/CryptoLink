namespace CryptoLink.Domain.Aggregates.Users.Enums;

public enum TokenStatus
{
    TokenNotReadyToResend = 0,
    TokenAlreadySent = 1,
    TokenExpired = 2,
    TokenReadyToResend = 3,
    TokenUsed = 4,
    TokenRefresh = 5,
}