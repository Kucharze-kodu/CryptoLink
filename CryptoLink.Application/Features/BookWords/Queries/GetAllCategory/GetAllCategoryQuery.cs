using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.BookWords.Queries.DTOs;


namespace CryptoLink.Application.Features.BookWords.Queries.GetAllCategory
{
    public class GetAllCategoryQuery(
        ) : ICommand<List<GetCategoryDto>>;
}
