using System;
using System.Threading;
using System.Threading.Tasks;

using ErrorOr;
using MediatR;
using Serilog;

using ExampleProject.Domain.ProfileAggregateRoot;
using ExampleProject.Application.Common.Contracts.Persistence;

namespace ExampleProject.Application.Profiles.Commands.Create;

/// <summary>
/// Handler for creating a <see cref="Profile"/>.
/// </summary>
public sealed class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ErrorOr<Guid>>
{
    private readonly IProfilesRepository _profilesRepository;
    private readonly ILogger _logger;

    /// <summary>
	/// Initializes a new instance of the <see cref="CreateProfileCommandHandler"/> class.
	/// </summary>
	/// <param name="profilesRepository"><see cref="IProfilesRepository"/>.</param>
	/// <param name="logger"><see cref="ILogger"/>.</param>
    public CreateProfileCommandHandler(IProfilesRepository profilesRepository, ILogger logger)
    {
        _profilesRepository = profilesRepository;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<ErrorOr<Guid>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = new Profile(Guid.NewGuid());
        return await _profilesRepository.CreateAsync(profile, cancellationToken);
    }
}

