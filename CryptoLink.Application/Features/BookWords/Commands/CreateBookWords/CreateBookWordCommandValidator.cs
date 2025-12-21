

using FluentValidation;

namespace CryptoLink.Application.Features.BookWords.Commands.CreateBookWords
{
    public class CreateBookWordCommandValidator : AbstractValidator<CreateBookWordCommand>
    {
        public CreateBookWordCommandValidator()
        {
            RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name is required");
            RuleFor(r => r.Category)
            .NotEmpty()
            .WithMessage("Category is required");
        }
    }
}
