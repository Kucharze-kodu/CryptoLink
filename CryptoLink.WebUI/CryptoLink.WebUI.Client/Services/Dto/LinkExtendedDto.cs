using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.WebUI.Client.Services.Dto
{
    public class LinkExtendedDto
    {
        public int Id { get; set; }
        public string UrlShort { get; set; } = string.Empty;
        public string UrlExtended { get; set; } = string.Empty;
        public DateTime CreatedOnUtc { get; set; }
        public DateTime ExpiretOnUtc { get; set; } 
    }
}
