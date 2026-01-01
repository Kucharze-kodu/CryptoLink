using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;


namespace CryptoLink.Application.Features.Users.Login
{
    public record LoginCommand(
        
        ) : ICommand<RegisterResponse>;
}
