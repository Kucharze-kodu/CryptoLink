using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;

namespace CryptoLink.Application.Features.BookWords.Commands.RandomLinks
{
    public record CreateRandomLinkCommand
    (
        int HowManyWord,
        List<string> Categories
        ) : ICommand<BookWordResponse>;
}
