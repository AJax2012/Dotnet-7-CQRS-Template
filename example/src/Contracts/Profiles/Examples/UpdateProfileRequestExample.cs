using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Filters;

using ExampleProject.Contracts.Profiles.Requests;

namespace ExampleProject.Contracts.Profiles.Examples;

/// <summary>
/// Example for <see cref="UpdateProfileRequest"/>.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Examples cannot be tested.")]
public class UpdateProfileRequestExample : IExamplesProvider<UpdateProfileRequest>
{
    /// <inheritdoc />
    public UpdateProfileRequest GetExamples()
    {
        return new UpdateProfileRequest
        {
        };
    }
}