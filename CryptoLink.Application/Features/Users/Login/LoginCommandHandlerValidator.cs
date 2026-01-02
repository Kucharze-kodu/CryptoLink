using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.Users.Login
{
    public class LoginCommandHandlerValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandHandlerValidator()
        {
        }

    }
}
