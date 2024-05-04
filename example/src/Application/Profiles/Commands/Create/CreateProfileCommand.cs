using System;
using System.Diagnostics.CodeAnalysis;

using ErrorOr;
using MediatR;

using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Commands.Create;

/// <summary>
/// Creates a <see cref="Profile"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record CreateProfileCommand() : IRequest<ErrorOr<Guid>>;
