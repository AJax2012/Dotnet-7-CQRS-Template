using System.Collections.Immutable;
using System.Linq;

using ExampleProject.Application.Profiles.Queries.GetPaginated;

namespace ExampleProject.Contracts.Profiles.Responses;

/// <summary>
/// Response for getting a paginated list of Profiles.
/// </summary>
public static class GetProfilesResponseFactory
{
    public static GetProfilesResponse ToResponse(this PaginatedProfilesDto dto) =>
        new(dto.Profiles.Select(x => x.ToResource()).ToImmutableList(), dto.NextPageToken);
        
    private static ProfileResource ToResource(this ProfileListItemDto dto) =>
        new(dto.Id);
}
