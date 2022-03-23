using FluentValidation;
using SourceName.Application.Commands;

namespace SourceName.Api.Validators;

public class DeleteExampleCommandValidator : AbstractValidator<DeleteExample.Command>
{
    public DeleteExampleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MinimumLength(36);
    }
}