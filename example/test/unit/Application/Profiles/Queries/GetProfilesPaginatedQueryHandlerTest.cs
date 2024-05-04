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
using ExampleProject.Application.Profiles.Queries.GetPaginated;

namespace ExampleProject.Application.Unit.Test.Profiles.Queries;

/// <summary>
/// Tests for <see cref="GetProfilesPaginatedQueryHandler"/>
/// </summary>
public class GetProfilesPaginatedQueryHandlerTest
{
	private readonly GetProfilesPaginatedQueryHandler _sut;
	private readonly IProfilesRepository _profilesRepository = Substitute.For<IProfilesRepository>();
	private readonly ILogger _logger = Substitute.For<ILogger>();
	
	/// <summary>
    /// Initializes a new instance of the <see cref="GetProfilesPaginatedQueryHandlerTest"/> class.
    /// </summary>
    public GetProfilesPaginatedQueryHandlerTest()
    {
        _sut = new GetProfilesPaginatedQueryHandler(_profilesRepository, _logger);
    }

    /// <summary>
    /// Should call GetPaginatedListAsync.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldCallGetPaginatedListAsync()
    {
        var request = new AutoFaker<GetProfilesPaginatedQuery>().Generate();
        await _sut.Handle(request, CancellationToken.None);
        await _profilesRepository.Received(1).GetPaginatedListAsync(request, CancellationToken.None);
    }
}
