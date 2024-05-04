using System;
using System.Threading;
using System.Threading.Tasks;

using ErrorOr;
using MediatR;
using Serilog;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Domain.ProfileAggregateRoot;

namespace ExampleProject.Application.Profiles.Commands.Update;

/// <summary>
/// Handler for deleting a <see cref="Profile"/>.
/// </summary>
public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ErrorOr<Updated>>
{
    private readonly IProfilesRepository _profilesRepository;
    private readonly ILogger _logger;

    /// <summary>
	/// Initializes a new instance of the <see cref="UpdateProfileCommandHandler"/> class.
	/// </summary>
	/// <param name="profilesRepository"><see cref="IProfilesRepository"/>.</param>
	/// <param name="logger"><see cref="ILogger"/>.</param>
    public UpdateProfileCommandHandler(IProfilesRepository profilesRepository, ILogger logger)
    {
        _profilesRepository = profilesRepository;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<ErrorOr<Updated>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profilesRepository.GetByIdAsync(request.ProfileId, cancellationToken);
                
        if (profile is null)
        {
            _logger.Error("Profile not found with id {ProfileId}.", request.ProfileId);
            return ProfileError.NotFound;
        }
        
        await _profilesRepository.UpdateAsync(profile, cancellationToken);
        return Result.Updated;
    }
}

