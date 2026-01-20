using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.LinkExtendeds.Queries.DTOs
{
    public class LoadLinkDto
    {
        public string Url { get; set; }
        public bool Capcha { get; set; }
    }
}
