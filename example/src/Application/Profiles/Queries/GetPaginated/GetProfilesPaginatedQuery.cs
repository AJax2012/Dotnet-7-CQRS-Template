using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ErrorOr;
using MediatR;

namespace ExampleProject.Application.Profiles.Queries.GetPaginated;

/// <summary>
/// Gets a paginated list of <see cref="Profile"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record GetProfilesPaginatedQuery(
    Guid CurrentUserId, 
    int Limit, 
    List<string> OrderBy, 
    bool IsDescending,
    string? NextPageToken) : IRequest<ErrorOr<PaginatedProfilesDto>>;
