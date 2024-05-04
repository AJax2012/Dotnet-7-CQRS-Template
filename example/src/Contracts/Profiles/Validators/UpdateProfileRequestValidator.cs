using FluentValidation;

using ExampleProject.Contracts.Profiles.Requests;

namespace ExampleProject.Contracts.Profiles.Validators;

/// <inheritdoc />
public partial class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    /// <inheritdoc />
    public UpdateProfileRequestValidator()
    {
    }
}
