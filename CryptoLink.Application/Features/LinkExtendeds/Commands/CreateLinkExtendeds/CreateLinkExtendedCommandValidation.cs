using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds
{
    public class CreateLinkExtendedCommandValidation : AbstractValidator<CreateLinkExtendedCommand>
    {
        public CreateLinkExtendedCommandValidation() 
        {
            RuleFor(r => r.UrlExtended)
                .NotEmpty()
                .WithMessage("Url Extende is required");
            RuleFor(r => r.ShortUrl)
                .NotEmpty()
                .WithMessage("Short url is required");
        }
    }
}
