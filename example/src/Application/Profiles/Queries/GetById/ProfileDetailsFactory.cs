using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Queries.GetById;

/// <summary>
/// Factory for creating <see cref="ProfileDetailsDto"/> from <see cref="Profile"/>.
/// </summary>
public static class ProfileDetailsFactory
{
    /// <summary>
    /// Converts a <see cref="Profile"/> to a <see cref="ProfileDetailsDto"/>.
    /// </summary>
    /// <param name="profile"><see cref="Profile"/>.</param>
    /// <returns></returns>
    public static ProfileDetailsDto ToDetailsDto(this Profile profile) =>
        new ProfileDetailsDto(profile.Id);
}
