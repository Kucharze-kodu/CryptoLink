using CryptoLink.Domain.Aggregates.Users.Enums;

namespace CryptoLink.Domain.Common.Interfaces;

public interface IToken
{
    bool Verify(Guid token);
}