using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.LinkExtendeds.Commands.DeleteLinkExntededs
{
    public record DeleteLinkExtendedCommand
    (

        ) : ICommand<LinkExtendedResponse>;
}
