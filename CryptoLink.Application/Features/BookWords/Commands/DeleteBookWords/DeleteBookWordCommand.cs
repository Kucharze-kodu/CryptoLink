using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.LinkWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.BookWords.Commands.DeleteBookWords
{
    public record DeleteBookWordCommand
    (
        
        ):ICommand<BookWordResponse>;
}
