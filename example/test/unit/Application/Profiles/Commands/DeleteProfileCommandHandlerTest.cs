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
using ExampleProject.Application.Profiles.Commands.Delete;

namespace ExampleProject.Application.Unit.Test.Profile.Commands;

/// <summary>
/// Tests for <see cref="DeleteProfileCommandHandler"/>.
/// </summary>
public class DeleteProfileCommandHandlerTest
{
    private readonly DeleteProfileCommandHandler _sut;
    private readonly IProfilesRepository _profilesRepository = Substitute.For<IProfilesRepository>();
    private readonly ILogger _logger = Substitute.For<ILogger>();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProfileCommandHandlerTest"/> class.
    /// </summary>
    public DeleteProfileCommandHandlerTest()
    {
        _sut = new DeleteProfileCommandHandler(_profilesRepository, _logger);
    }
}

