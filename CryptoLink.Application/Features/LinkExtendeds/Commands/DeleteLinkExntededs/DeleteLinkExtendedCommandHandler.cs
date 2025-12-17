using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkExtendeds;
using CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
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

        public async Task<ErrorOr<LinkExtendedResponse>> Handle(DeleteLinkExtendedCommand request, CancellationToken cancellationToken)
        {





            return new LinkExtendedResponse("link delete");
        }
    }
}
