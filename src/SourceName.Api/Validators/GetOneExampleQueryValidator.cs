using FluentValidation;
using SourceName.Application.Queries;

namespace SourceName.Api.Validators;

public class GetOneExampleQueryValidator : AbstractValidator<GetOneExample.Query>
{
    public GetOneExampleQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Length(36);
    }
}