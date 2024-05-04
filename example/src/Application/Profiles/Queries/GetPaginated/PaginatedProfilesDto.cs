using System.Collections.Generic;

namespace ExampleProject.Application.Profiles.Queries.GetPaginated;

/// <summary>
/// Paginated list of Profiles and NextPage for pagination.
/// </summary>
/// <param name="Profiles"><see cref="ProfileListItemDto"/> list.</param>
/// <param name="NextPage">Token generated from <see cref="ProfileNextPageDto"/>.</param>
public record PaginatedProfilesDto(IReadOnlyList<ProfileListItemDto> Profiles, string NextPageToken);