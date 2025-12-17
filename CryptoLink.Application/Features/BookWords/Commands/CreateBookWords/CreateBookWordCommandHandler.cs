using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.BookWords.Commands.CreateBookWords
{
    public class CreateBookWordCommandHandler : ICommandHandler<CreateBookWordCommand, BookWordResponse>
    {
        private readonly IBookWordRepository _bookWordRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookWordCommandHandler(IBookWordRepository bookWordRepository, IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _bookWordRepository = bookWordRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }


        public async Task<ErrorOr<BookWordResponse>> Handle(CreateBookWordCommand request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.LinkExtended.IsNotAuthorized;
            }

            // var userId = _userContext.GetUserId();







            return new BookWordResponse("word to book added");
        }
    }
}
