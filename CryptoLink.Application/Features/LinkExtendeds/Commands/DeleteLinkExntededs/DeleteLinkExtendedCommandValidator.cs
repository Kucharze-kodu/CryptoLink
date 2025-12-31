using FluentValidation;

namespace CryptoLink.Application.Features.LinkExtendeds.Commands.DeleteLinkExntededs
{
    public class DeleteLinkExtendedCommandValidator : AbstractValidator<DeleteLinkExtendedCommand>
    {
        public DeleteLinkExtendedCommandValidator()
        {
            RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        }
    }
}
