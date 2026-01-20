using FluentValidation;

namespace CryptoLink.Application.Features.BookWords.Commands.RandomLinks
{
    public class CreateRandomLinkCommandValidator : AbstractValidator<CreateRandomLinkCommand>
    {
        public CreateRandomLinkCommandValidator()
        {
            RuleFor(r => r.HowManyWord >= 10)
            .NotEmpty()
            .WithMessage("You must select more than 10 but better 100");
            RuleFor(r => r.Categories)
            .NotEmpty()
            .WithMessage("One category must be");
        }
    }
}
