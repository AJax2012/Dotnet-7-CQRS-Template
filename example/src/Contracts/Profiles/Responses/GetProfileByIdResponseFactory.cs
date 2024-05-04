using ExampleProject.Application.Profiles.Queries.GetById;

namespace ExampleProject.Contracts.Profiles.Responses;

/// <summary>
/// Represents the response returned when a Profile is retrieved by its identifier.
/// </summary>
public static class GetProfileByIdResponseFactory
{
    public static GetProfileByIdResponse ToResponse(this ProfileDetailsDto dto) =>
        new(dto.Id);
}
