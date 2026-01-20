using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.LinkExtendeds.Queries.DTOs;


namespace CryptoLink.Application.Features.LinkExtendeds.Queries.LoadLinkExtended
{
    public record LoadLinkExtendedQuery(
        string Link
        ) : ICommand<LoadLinkDto>;
}
