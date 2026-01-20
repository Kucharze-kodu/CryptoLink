using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.LinkExtendeds.Queries.DTOs;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.LinkExtendeds.Queries.LoadLinkExtended
{
    public class LoadLinkExtendedQueryHandler : ICommandHandler<LoadLinkExtendedQuery, LoadLinkDto>
    {
        private readonly ILinkExtendedRepository _linkExtendedRepository;
        private readonly IUserContext _userContext;

        public LoadLinkExtendedQueryHandler(ILinkExtendedRepository linkExtendedRepository, IUserContext userContext)
        {
            _linkExtendedRepository = linkExtendedRepository;
            _userContext = userContext;
        }

        async  public Task<ErrorOr<LoadLinkDto>> Handle(LoadLinkExtendedQuery request, CancellationToken cancellationToken)
        {

            var result = await _linkExtendedRepository.LoadLinkExtended(request.Link, cancellationToken);
            if (result == null)
            {
                return Errors.LinkExtended.IsWrongData;
            }

            var random = new Random();
            double fate = random.NextDouble();


            var dto = new LoadLinkDto
            {
                Url = result,
                Capcha = fate > 0.5
            };

            return dto;
        }
    }
}
