using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Filters;

using ExampleProject.Contracts.Profiles.Responses;

namespace ExampleProject.Contracts.Profiles.Examples;

/// <summary>
/// Example for <see cref="GetProfilesResponse"/>.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Examples cannot be tested.")]
public class GetProfilesResponseExample : IExamplesProvider<GetProfilesResponse>
{
    /// <inheritdoc />
    public GetProfilesResponse GetExamples()
    {
        return new GetProfilesResponse(
            new List<ProfileResource>
            {
                new(Guid.NewGuid()),
            }.ToImmutableList(), 
            "nextPageToken");
    }
}