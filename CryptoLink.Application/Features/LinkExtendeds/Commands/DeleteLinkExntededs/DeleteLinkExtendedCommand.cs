using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;

namespace CryptoLink.Application.Features.LinkExtendeds.Commands.DeleteLinkExntededs
{
    public record DeleteLinkExtendedCommand
    (
        int Id
        ) : ICommand<LinkExtendedResponse>;
}
