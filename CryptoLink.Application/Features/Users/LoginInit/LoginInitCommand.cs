using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;

namespace CryptoLink.Application.Features.Users.LoginInit
{
    public record LoginInitCommand(
        string UserName
        ) : ICommand<LoginInitResponse>;
}
