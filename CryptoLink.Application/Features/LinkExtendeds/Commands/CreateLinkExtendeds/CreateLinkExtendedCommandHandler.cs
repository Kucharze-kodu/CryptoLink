using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using ErrorOr;
using CryptoLink.Domain.Common.Errors;


namespace CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds
{
    public class CreateLinkExtendedCommandHandler : ICommandHandler<CreateLinkExtendedCommand, LinkExtendedResponse>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLinkExtendedCommandHandler(
            ILinkExtendedRepository linkExtendedRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork )
        {
            _linkExtendedRepository = linkExtendedRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }




        public async Task<ErrorOr<LinkExtendedResponse>> Handle(CreateLinkExtendedCommand request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.LinkExtended.IsNotAuthorized;
            }

            // var userId = _userContext.GetUserId();





            return new LinkExtendedResponse("create message");
        }
    }
}
