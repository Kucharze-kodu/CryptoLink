using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.BookWords.Queries.DTOs;



namespace CryptoLink.Application.Features.BookWords.Queries.GetAllBookWordCategory
{
    public record GetAllBookWordCategoryQuery(
        List<string> Categories
        ) : ICommand<List<GetBookWordDto>>;
}
