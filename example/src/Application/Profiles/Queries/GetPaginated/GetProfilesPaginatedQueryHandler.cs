using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ErrorOr;
using MediatR;
using Serilog;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Queries.GetPaginated;

/// <summary>
/// Handler for getting a paginated list of <see cref="Profile"/>.
/// </summary>
public sealed class GetProfilesPaginatedQueryHandler : IRequestHandler<GetProfilesPaginatedQuery, ErrorOr<PaginatedProfilesDto>>
{
    private readonly IProfilesRepository _profilesRepository;
    private readonly ILogger _logger;

    /// <summary>
	/// Initializes a new instance of the <see cref="GetProfilesPaginatedQueryHandler"/> class.
	/// </summary>
	/// <param name="profilesRepository"><see cref="IProfilesRepository"/>.</param>
	/// <param name="logger"><see cref="ILogger"/>.</param>
    public GetProfilesPaginatedQueryHandler(IProfilesRepository profilesRepository, ILogger logger)
    {
        _profilesRepository = profilesRepository;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<ErrorOr<PaginatedProfilesDto>> Handle(GetProfilesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _profilesRepository.GetPaginatedListAsync(request, cancellationToken);
        
        if (profiles is null || !profiles.Any())
        {
            _logger.Error("Failed to get paginated list of Profile.");
            return ProfileError.NotFound;
        }
        
        return profiles.ToPaginatedDto();
    }
}

