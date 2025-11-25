using CryptoLink.Domain.Aggregates.Users;

namespace CryptoLink.Application.Utils;

public interface IJwtProvider
{
    IJwtBearerToken GenerateToken(User user);
}