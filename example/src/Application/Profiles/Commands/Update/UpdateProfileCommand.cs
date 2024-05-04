using System;
using System.Diagnostics.CodeAnalysis;

using ErrorOr;
using MediatR;

using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Commands.Update;

/// <summary>
/// Updates <see cref="Profile"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record UpdateProfileCommand(Guid ProfileId, Guid CurrentUserId) : IRequest<ErrorOr<Updated>>;
