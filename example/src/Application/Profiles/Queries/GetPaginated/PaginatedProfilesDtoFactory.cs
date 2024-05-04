using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using ExampleProject.Application.Common.Services;
using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Queries.GetPaginated;

/// <summary>
/// Generates a paginated list of Profiles for the application layer.
/// </summary>
public static class PaginatedProfilesDtoFactory
{
    /// <summary>
    /// Generates a paginated list of Profiles.
    /// </summary>
	public static PaginatedProfilesDto ToPaginatedDto(this IReadOnlyList<Profile> profiles)
	{
	    var lastItem = profiles.LastOrDefault();
	    var nextPageDto = new PaginatedProfilesNextPageDto();
	    
	    return new PaginatedProfilesDto(Profiles: profiles.ToDtoList(), NextPageToken: nextPageDto.EncodeNextPageToken());
    }

	private static IReadOnlyList<ProfileListItemDto> ToDtoList(this IEnumerable<Profile> profiles) =>
	    profiles.Select(x => x.ToListItemDto()).ToImmutableList();

	private static ProfileListItemDto ToListItemDto(this Profile profile) =>
	    new(profile.Id);
}