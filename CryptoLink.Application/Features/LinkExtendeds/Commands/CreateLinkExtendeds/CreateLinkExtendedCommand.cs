using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;


namespace CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds
{
    public record CreateLinkExtendedCommand
    (
        string UrlExtended,
        string ShortUrl
        ) : ICommand<LinkExtendedResponse>;
}
