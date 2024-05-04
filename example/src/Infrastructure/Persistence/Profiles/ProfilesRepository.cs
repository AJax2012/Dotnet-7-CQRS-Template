using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Application.Profiles.Queries.GetPaginated;
using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Infrastructure.Persistence.Profiles;

/// <inheritdoc />
public class ProfilesRepository : IProfilesRepository
{
    /// <inheritdoc />
    public Task<Guid> CreateAsync(Profile profile, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Profile?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ImmutableList<Profile>?> GetPaginatedListAsync(GetProfilesPaginatedQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task UpdateAsync(Profile profile, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
