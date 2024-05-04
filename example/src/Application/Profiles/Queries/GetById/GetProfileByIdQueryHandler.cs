using System;
using System.Threading;
using System.Threading.Tasks;

using ErrorOr;
using MediatR;
using Serilog;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Queries.GetById;

/// <summary>
/// Handler for getting a <see cref="Profile"/> by unique identifier.
/// </summary>
public sealed class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, ErrorOr<ProfileDetailsDto>>
{
    private readonly IProfilesRepository _profilesRepository;
    private readonly ILogger _logger;

    /// <summary>
	/// Initializes a new instance of the <see cref="GetProfileByIdQueryHandler"/> class.
	/// </summary>
	/// <param name="profilesRepository"><see cref="IProfilesRepository"/>.</param>
	/// <param name="logger"><see cref="ILogger"/>.</param>
    public GetProfileByIdQueryHandler(IProfilesRepository profilesRepository, ILogger logger)
    {
        _profilesRepository = profilesRepository;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<ErrorOr<ProfileDetailsDto>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profilesRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile == null)
        {
            _logger.Information("Profile with id {ProfileId} not found", request.ProfileId);
            return ProfileError.NotFound;
        }
        
        return profile.ToDetailsDto();
    }
}

