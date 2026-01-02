using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Contracts.Users
{
    public record RegisterInitResponse
    (
        string Challenge
    );
}
