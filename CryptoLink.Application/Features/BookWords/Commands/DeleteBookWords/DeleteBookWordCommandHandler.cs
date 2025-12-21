using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using ErrorOr;
using CryptoLink.Domain.Common.Errors;
using CryptoLink.Domain.Aggregates.BookWords.ValueObcjets;


namespace CryptoLink.Application.Features.BookWords.Commands.DeleteBookWords
{
    public class DeleteBookWordCommandHandler : ICommandHandler<DeleteBookWordCommand, BookWordResponse>
    {
        private readonly IBookWordRepository _bookWordRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;


        public DeleteBookWordCommandHandler(IBookWordRepository bookWordRepository, IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _bookWordRepository = bookWordRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }



        public async Task<ErrorOr<BookWordResponse>> Handle(DeleteBookWordCommand request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.BookWord.IsNotAuthorized;
            }

            var id = _userContext.UserId;
            UserId userId = UserId.Create(Convert.ToInt32(id));
            BookWordId bookWordId = BookWordId.Create(request.Id);

            await _bookWordRepository.RemoveBookWord(bookWordId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookWordResponse("delete word from book");
        }
    }

}
