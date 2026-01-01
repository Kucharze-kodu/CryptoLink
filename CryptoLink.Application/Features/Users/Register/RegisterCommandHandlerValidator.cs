using CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.Users.Register
{
    public class RegisterCommandHandlerValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandHandlerValidator()
        {
            RuleFor(r => r.PublicKey)
                .NotEmpty()
                .WithMessage("Public key is required");
        }
    }
}