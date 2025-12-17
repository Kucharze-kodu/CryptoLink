using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using ErrorOr;


namespace CryptoLink.Application.Features.LinkExtendeds.Commands.EditLinkExtendeds
{
    public class EditLinkExtendedCommandHandler : ICommandHandler<EditLinkExtendedCommand, LinkExtendedResponse>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public async Task<ErrorOr<LinkExtendedResponse>> Handle(EditLinkExtendedCommand request, CancellationToken cancellationToken)
        {






            return new LinkExtendedResponse("link edited");
        }
    }
}
