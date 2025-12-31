using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.BookWords.Queries.DTOs;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;


namespace CryptoLink.Application.Features.BookWords.Queries.GetAllBookWordCategory
{
    public class GetAllBookWordCategoryQueryHandler : ICommandHandler<GetAllBookWordCategoryQuery, List<GetBookWordDto>>
    {
        private readonly IBookWordRepository _bookWordRepository;
        private readonly IUserContext _userContext;


        public GetAllBookWordCategoryQueryHandler(IBookWordRepository bookWordRepository, IUserContext userContext)
        {
            _bookWordRepository = bookWordRepository;
            _userContext = userContext;
        }



        public async Task<ErrorOr<List<GetBookWordDto>>> Handle(GetAllBookWordCategoryQuery request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.BookWord.IsNotAuthorized;
            }

            var result = await _bookWordRepository.GetAllBookWordCategory(request.Categories, cancellationToken);


            List<GetBookWordDto> listDto = result.Select(x => new GetBookWordDto
            {
                Id = Convert.ToInt32(x.Id.Value),
                Name = x.Word
            }).ToList();

            return listDto;
        }
    }
}
