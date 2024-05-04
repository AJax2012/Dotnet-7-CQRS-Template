using System;

namespace ExampleProject.Domain.ProfileAggregateRoot;

/// <summary>
/// Profile AggregateRoot.
/// </summary>
public class Profile 
{
    /// <summary>
    /// Profile Id.
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Profile"/> class.
    /// </summary>
    public Profile(Guid id)
    {
        Id = id;
    }
}
