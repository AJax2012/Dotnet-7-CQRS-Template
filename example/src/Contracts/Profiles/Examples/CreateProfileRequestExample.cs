using System.Diagnostics.CodeAnalysis;

using Swashbuckle.AspNetCore.Filters;

using ExampleProject.Contracts.Profiles.Requests;

namespace ExampleProject.Contracts.Profiles.Examples;

/// <summary>
/// Example for <see cref="CreateProfileRequest"/>.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Examples cannot be tested.")]
public class CreateProfileRequestExample : IExamplesProvider<CreateProfileRequest>
{
    /// <inheritdoc />
    public CreateProfileRequest GetExamples()
    {
        return new CreateProfileRequest();
    }
}