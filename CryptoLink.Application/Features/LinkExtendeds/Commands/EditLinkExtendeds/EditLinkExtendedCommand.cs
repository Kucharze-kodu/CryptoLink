using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;

namespace CryptoLink.Application.Features.LinkExtendeds.Commands.EditLinkExtendeds
{
    public record EditLinkExtendedCommand
    (
        
        ): ICommand<LinkExtendedResponse>;
}
