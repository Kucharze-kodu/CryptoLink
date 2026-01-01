using FluentValidation;


namespace CryptoLink.Application.Features.Users.RegisterInit
{
    public class RegisterInitCommnandValidator : AbstractValidator<RegisterInitCommnand>
    {
        public RegisterInitCommnandValidator()
        {
            RuleFor(r => r.PublicKey)
                .NotEmpty()
                .WithMessage("Public key is required");
        }
    }
}
