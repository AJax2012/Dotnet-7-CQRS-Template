using FluentValidation;
using SourceName.Application.Commands;

namespace SourceName.Api.Validators;

public class CreateExampleCommandValidator : AbstractValidator<CreateExample.CreateCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(1);
    }
}