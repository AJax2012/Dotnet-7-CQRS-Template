using System.Threading;
using System.Threading.Tasks;

using AutoBogus;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Serilog;
using Xunit;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Application.Profiles.Queries.GetById;

namespace ExampleProject.Application.Unit.Test.Profiles.Queries;

/// <summary>
/// Tests for <see cref="GetProfileByIdQueryHandler"/>
/// </summary>
public class GetProfileByIdQueryHandlerTest
{
	private readonly GetProfileByIdQueryHandler _sut;
	private readonly IProfilesRepository _profilesRepository = Substitute.For<IProfilesRepository>();
	private readonly ILogger _logger = Substitute.For<ILogger>();
	
	/// <summary>
    /// Initializes a new instance of the <see cref="GetProfileByIdQueryHandlerTest"/> class.
    /// </summary>
    public GetProfileByIdQueryHandlerTest()
    {
        _sut = new GetProfileByIdQueryHandler(_profilesRepository, _logger);
    }

    /// <summary>
    /// Should call GetByIdAsync.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldCallGetByIdAsync()
    {
        var request = new AutoFaker<GetProfileByIdQuery>().Generate();
        await _sut.Handle(request, CancellationToken.None);
        await _profilesRepository.Received(1).GetByIdAsync(request.ProfileId, CancellationToken.None);
    }
}
