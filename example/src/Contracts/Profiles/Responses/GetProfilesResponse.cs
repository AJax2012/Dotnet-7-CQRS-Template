using System.Collections.Immutable;

namespace ExampleProject.Contracts.Profiles.Responses;

/// <summary>
/// Response for getting a paginated list of Profiles.
/// </summary>
public record GetProfilesResponse(ImmutableList<ProfileResource> Profiles, string NextPageToken);
