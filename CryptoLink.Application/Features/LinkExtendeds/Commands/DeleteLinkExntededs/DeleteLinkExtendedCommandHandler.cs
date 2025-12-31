using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;
using CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.LinkExtendeds.Commands.DeleteLinkExntededs
{
    public class DeleteLinkExtendedCommandHandler : ICommandHandler<DeleteLinkExtendedCommand, LinkExtendedResponse>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLinkExtendedCommandHandler(ILinkExtendedRepository linkExtendedRepository, IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _linkExtendedRepository = linkExtendedRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<LinkExtendedResponse>> Handle(DeleteLinkExtendedCommand request, CancellationToken cancellationToken)
        {


            var id = _userContext.UserId;
            UserId userId = UserId.Create(Convert.ToInt32(id));
            LinkExtendedId linkExtebded = LinkExtendedId.Create(request.Id);

            await _linkExtendedRepository.DeleteLinkExntended(userId,linkExtebded, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new LinkExtendedResponse("link delete");
        }
    }
}
