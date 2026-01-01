using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;

namespace CryptoLink.Application.Features.Users.Register
{
    public record RegisterCommand(
        string UserName,
        string DecryptedToken
        ) : ICommand<RegisterResponse>;
}
