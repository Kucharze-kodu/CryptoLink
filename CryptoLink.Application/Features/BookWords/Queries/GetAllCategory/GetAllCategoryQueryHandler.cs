using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.BookWords.Queries.DTOs;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.BookWords.Queries.GetAllCategory
{
    public class GetAllCategoryQueryHandler : ICommandHandler<GetAllCategoryQuery, List<GetCategoryDto>>
    {
        private readonly IBookWordRepository _bookWordRepository;
        private readonly IUserContext _userContext;

        public GetAllCategoryQueryHandler(IBookWordRepository bookWordRepository, IUserContext userContext)
        {
            _bookWordRepository = bookWordRepository;
            _userContext = userContext;
        }


        public async Task<ErrorOr<List<GetCategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.BookWord.IsNotAuthorized;
            }

            var result = await _bookWordRepository.GetAllCategory(cancellationToken);

            List<GetCategoryDto> listDto = result.Select(x => new GetCategoryDto
            {
                Name = x
            }).ToList();

            return listDto;
        }
    }
}
