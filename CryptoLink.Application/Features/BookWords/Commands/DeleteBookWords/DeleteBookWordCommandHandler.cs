using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using ErrorOr;

namespace CryptoLink.Application.Features.BookWords.Commands.DeleteBookWords
{
    public class DeleteBookWordCommandHandler : ICommandHandler<DeleteBookWordCommand, BookWordResponse>
    {
        private readonly IBookWordRepository bookWordRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ErrorOr<BookWordResponse>> Handle(DeleteBookWordCommand request, CancellationToken cancellationToken)
        {





            return new BookWordResponse("delete word from book");
        }
    }

}
