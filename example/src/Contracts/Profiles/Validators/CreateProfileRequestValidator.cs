using FluentValidation;

using ExampleProject.Contracts.Profiles.Requests;

namespace ExampleProject.Contracts.Profiles.Validators;

/// <inheritdoc />
public partial class CreateProfileRequestValidator : AbstractValidator<CreateProfileRequest>
{
    /// <inheritdoc />
    public CreateProfileRequestValidator()
    {
    }
}
