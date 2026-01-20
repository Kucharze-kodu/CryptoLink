using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Features.LinkExtendeds.Queries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.LinkExtendeds.Queries.GetLinkExtended
{
    public record GetLinkExtendedQuery(
        int Id
        ) : ICommand<GetLinkExtendedDto>;
}
