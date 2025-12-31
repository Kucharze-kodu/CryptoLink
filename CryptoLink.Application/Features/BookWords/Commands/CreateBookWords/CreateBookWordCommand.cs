using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;


namespace CryptoLink.Application.Features.BookWords.Commands.CreateBookWords
{
    public record CreateBookWordCommand
    (
        string Name,
        string Category
        ):ICommand<BookWordResponse>;
}
