using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Aggregates.LinkExtendeds;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;



namespace CryptoLink.Application.Features.LinkExtendeds.Commands.EditLinkExtendeds
{
    public class EditLinkExtendedCommandHandler : ICommandHandler<EditLinkExtendedCommand, LinkExtendedResponse>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public EditLinkExtendedCommandHandler(
            ILinkExtendedRepository linkExtendedRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _linkExtendedRepository = linkExtendedRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }


        public async Task<ErrorOr<LinkExtendedResponse>> Handle(EditLinkExtendedCommand request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.LinkExtended.IsNotAuthorized;
            }

            var id = _userContext.UserId;
            UserId userId = UserId.Create(Convert.ToInt32(id));
            LinkExtendedId linkExtebdedId = LinkExtendedId.Create(request.Id);

            await _linkExtendedRepository.EditLinkExntended(userId, linkExtebdedId, request.UrlExtended,request.DataExpire, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LinkExtendedResponse("link edited");
        }
    }
}
