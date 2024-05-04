using System;

namespace ExampleProject.Contracts.Profiles.Responses;

/// <summary>
/// Response for getting a Profile by Id.
/// </summary>
public record GetProfileByIdResponse(Guid Id);