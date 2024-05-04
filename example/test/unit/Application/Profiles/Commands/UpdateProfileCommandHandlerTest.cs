using System.Threading;
using System.Threading.Tasks;

using AutoBogus;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;
using Xunit;

using ExampleProject.Application.Common.Contracts.Persistence;
using ExampleProject.Application.Profiles.Commands.Update;

namespace ExampleProject.Application.Unit.Test.Profile.Commands;

/// <summary>
/// Tests for <see cref="UpdateProfileCommandHandler"/>.
/// </summary>
public class UpdateProfileCommandHandlerTest
{
    private readonly UpdateProfileCommandHandler _sut;
    private readonly IProfilesRepository _profilesRepository = Substitute.For<IProfilesRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileCommandHandlerTest"/> class.
    /// </summary>
    public UpdateProfileCommandHandlerTest()
    {
        _sut = new UpdateProfileCommandHandler(_profilesRepository, _logger);
    }
}

