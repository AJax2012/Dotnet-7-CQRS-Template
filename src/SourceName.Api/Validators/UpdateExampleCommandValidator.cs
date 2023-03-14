using FluentValidation;
using SourceName.Application.Commands;

namespace SourceName.Api.Validators;

public class UpdateExampleCommandValidator : AbstractValidator<UpdateExample.UpdateCommand>
{
    public UpdateExampleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MinimumLength(36);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(1);
    }
}