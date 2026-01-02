using FluentValidation;


namespace CryptoLink.Application.Features.Users.RegisterInit
{
    public class RegisterInitCommandValidator : AbstractValidator<RegisterInitCommand>
    {
        public RegisterInitCommandValidator()
        {
            RuleFor(r => r.PublicKey)
                .NotEmpty()
                .WithMessage("Public key is required");
        }
    }
}
