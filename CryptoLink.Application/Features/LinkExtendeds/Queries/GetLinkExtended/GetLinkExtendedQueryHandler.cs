using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.LinkExtendeds.Queries.DTOs;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.LinkExtendeds.Queries.GetLinkExtended
{
    public class GetLinkExtendedQueryHandler : ICommandHandler<GetLinkExtendedQuery, GetLinkExtendedDto>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;

        public GetLinkExtendedQueryHandler(ILinkExtendedRepository linkExtendedRepository, IUserContext userContext)
        {
            _linkExtendedRepository = linkExtendedRepository;
            _userContext = userContext;
        }

        public async Task<ErrorOr<GetLinkExtendedDto>> Handle(GetLinkExtendedQuery request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.LinkExtended.IsNotAuthorized;
            }

            var id = _userContext.UserId;
            UserId userId = UserId.Create(Convert.ToInt32(id));
            LinkExtendedId linkExtendedId = LinkExtendedId.Create(request.Id);

            var result = await _linkExtendedRepository.GetLinkExtended(userId, linkExtendedId, cancellationToken);

            GetLinkExtendedDto dto = new GetLinkExtendedDto
            {
                Id = result.Id.Value,
                UrlShort = result.UrlShort,
                UrlExtended = result.UrlExtended,
                ExpiretOnUtc = result.ExpiretOnUtc
            };

            return dto;
        }
    }
}
