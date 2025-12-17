using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using ErrorOr;


namespace CryptoLink.Application.Features.BookWords.Commands.RandomLinks
{
    public class CreateRandomLinkCommandHandle : ICommandHandler<CreateRandomLinkCommand, BookWordResponse>
    {
        private readonly IBookWordRepository bookWordRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ErrorOr<BookWordResponse>> Handle(CreateRandomLinkCommand request, CancellationToken cancellationToken)
        {



            return new BookWordResponse("random link");
        }
    }
}
