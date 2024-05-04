using System.Threading;
using System.Threading.Tasks;

using Bogus;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;
using Xunit;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Application.Profiles.Commands.Create;

namespace ExampleProject.Application.Unit.Test.Profile.Commands;

/// <summary>
/// Tests for <see cref="CreateProfileCommandHandler"/>.
/// </summary>
public class CreateProfileCommandHandlerTest
{
    private readonly CreateProfileCommandHandler _sut;
    private readonly IProfilesRepository _profilesRepository = Substitute.For<IProfilesRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProfileCommandHandlerTest"/> class.
    /// </summary>
    public CreateProfileCommandHandlerTest()
    {
        _sut = new CreateProfileCommandHandler(_profilesRepository, _logger);
    }
}
