using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;

namespace CryptoLink.Application.Features.Users.RegisterInit
{
    public record RegisterInitCommnand(
        string UserName,
        string PublicKey
        ): ICommand<RegisterInitResponse>;
}
