using ErrorOr;

namespace ExampleProject.Domain.ProfileAggregateRoot;

/// <summary>
/// List of Domain <see cref="Error"/>s for <see cref="Profile"/>.
/// </summary>
public static class ProfileError
{
    /// <summary>
    /// Profile was not found.
    /// </summary>
    public readonly static Error NotFound = Error.NotFound("Profile.NotFound", "Profile was not found.");
}
