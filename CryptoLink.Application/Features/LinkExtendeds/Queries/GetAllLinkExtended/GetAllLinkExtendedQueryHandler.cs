using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.LinkExtendeds.Queries.DTOs;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.LinkExtendeds.Queries.GetAllLinkExtended
{
    public class GetAllLinkExtendedQueryHandler : ICommandHandler<GetAllLinkExtendedQuery, List<GetLinkExtendedDto>>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;

        public GetAllLinkExtendedQueryHandler(ILinkExtendedRepository linkExtendedRepository, IUserContext userContext)
        {
            _linkExtendedRepository = linkExtendedRepository;
            _userContext = userContext;
        }

        public async Task<ErrorOr<List<GetLinkExtendedDto>>> Handle(GetAllLinkExtendedQuery request, CancellationToken cancellationToken)
        {
            var isVerify = _userContext.IsAuthenticated;
            if (isVerify == false)
            {
                return Errors.BookWord.IsNotAuthorized;
            }

            var id = _userContext.UserId;
            UserId userId = UserId.Create(Convert.ToInt32(id));

            var result = await _linkExtendedRepository.GetAllLinkExntended(userId, cancellationToken);

            List<GetLinkExtendedDto> listDto = result.Select(x => new GetLinkExtendedDto
            {
                Id = x.Id.Value,
                UrlShort = x.UrlShort,
                UrlExtended = x.UrlExtended,
                CreatedOnUtc = x.CreatedOnUtc,
                ExpiretOnUtc = x.ExpiretOnUtc

            }).ToList();

            return listDto;

        }
    }
}
