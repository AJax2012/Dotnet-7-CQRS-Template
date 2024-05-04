using System;
using System.Diagnostics.CodeAnalysis;

using ErrorOr;
using MediatR;

namespace ExampleProject.Application.Profiles.Queries.GetById;

/// <summary>
/// Gets a <see cref="Profile"/> by unique identifier.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record GetProfileByIdQuery(Guid ProfileId, Guid CurrentUserId) : IRequest<ErrorOr<ProfileDetailsDto>>;
