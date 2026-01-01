using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.Users.Register
{
    public record RegisterCommand(
        string PublicKey
        ) : ICommand<RegisterResponse>;
}
