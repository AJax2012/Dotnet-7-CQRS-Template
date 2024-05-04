using System;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Filters;

using ExampleProject.Contracts.Profiles.Responses;

namespace ExampleProject.Contracts.Profiles.Examples;

/// <summary>
/// Example for <see cref="GetProfileByIdResponse"/>.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Examples cannot be tested.")]
public class GetProfileByIdResponseExample : IExamplesProvider<GetProfileByIdResponse>
{
    /// <inheritdoc />
    public GetProfileByIdResponse GetExamples()
    {
        return new GetProfileByIdResponse(Guid.NewGuid());
    }
}