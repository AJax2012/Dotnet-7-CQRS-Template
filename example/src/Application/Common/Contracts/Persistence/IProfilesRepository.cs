using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Immutable;

using ExampleProject.Application.Profiles.Queries.GetPaginated;
using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Common.Contracts.Persistence;

/// <summary>
/// Persistence Repository for <see cref="Profile"/>.
/// </summary>
public interface IProfilesRepository
{
    /// <summary>
	/// Creates <see cref="Profile"/>.
	/// </summary>
	/// <param name="profile"><see cref="Profile"/>.</param>
	/// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
	/// <returns><see cref="Guid"/> Id.</returns>
	Task<Guid> CreateAsync(Profile profile, CancellationToken cancellationToken);
	
    /// <summary>
    /// Get <see cref="Profile"/> by Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Profile"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Profile"/> entity.</returns>
    Task<Profile?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get paginated <see cref="Profile"/> entities.
    /// </summary>
    /// <param name="query">The query to get paginated <see cref="Profile"/> entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of <see cref="Profile"/> entities.</returns>
    Task<ImmutableList<Profile>?> GetPaginatedListAsync(GetProfilesPaginatedQuery query, CancellationToken cancellationToken);
    
    /// <summary>
    /// Updates <see cref="Profile"/> by id.
    /// </summary>
    /// <param name="Profile"><see cref="Profile"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Task.</returns>
    Task UpdateAsync(Profile profile, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes <see cref="Profile"/> by id.
    /// </summary>
    /// <param name="id"><see cref="Profile"/> id.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Task.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}