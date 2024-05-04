using System;
using System.Diagnostics.CodeAnalysis;

using ErrorOr;
using MediatR;

using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Commands.Delete;

/// <summary>
/// Deletes <see cref="Profile"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record DeleteProfileCommand(Guid ProfileId, Guid CurrentUserId) : IRequest<ErrorOr<Deleted>>;
