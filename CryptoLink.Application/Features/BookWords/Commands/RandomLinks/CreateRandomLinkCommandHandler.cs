using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using ErrorOr;
using CryptoLink.Domain.Common.Errors;

namespace CryptoLink.Application.Features.BookWords.Commands.RandomLinks
{
    public class CreateRandomLinkCommandHandle : ICommandHandler<CreateRandomLinkCommand, BookWordResponse>
    {
        private readonly IBookWordRepository _bookWordRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRandomLinkCommandHandle(IBookWordRepository bookWordRepository, IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _bookWordRepository = bookWordRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }


        public async Task<ErrorOr<BookWordResponse>> Handle(CreateRandomLinkCommand request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.BookWord.IsNotAuthorized;
            }

            var randomLink = await _bookWordRepository.RandomLink(request.HowManyWord, request.Categories, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookWordResponse("random link is:" + randomLink);
        }
    }
}
