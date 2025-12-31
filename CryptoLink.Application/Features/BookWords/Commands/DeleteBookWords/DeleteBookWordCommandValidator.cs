using FluentValidation;


namespace CryptoLink.Application.Features.BookWords.Commands.DeleteBookWords
{
    public class DeleteBookWordCommandValidator : AbstractValidator<DeleteBookWordCommand>
    {
        public DeleteBookWordCommandValidator()
        {
            RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        }
    }
}
