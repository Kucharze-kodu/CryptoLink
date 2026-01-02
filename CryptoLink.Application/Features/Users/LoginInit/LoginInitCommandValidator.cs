using FluentValidation;


namespace CryptoLink.Application.Features.Users.LoginInit
{
    public class LoginInitCommandValidator : AbstractValidator<LoginInitCommand>
    {
        public LoginInitCommandValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty()
                .WithMessage("UserName is required");
        }
    }
}
