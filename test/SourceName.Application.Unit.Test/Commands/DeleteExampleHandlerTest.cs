using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Application.Test.Commands;

[TestFixture]
public class DeleteExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private Mock<ICurrentUserService> _currentUserServiceMock = null!;
    private DeleteExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _sut = new DeleteExample.Handler(_repositoryMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public async Task Should_Call_CurrentUserService_Username()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new DeleteExample.Command(entity.Id);

        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(s => s == entity.Id)))
            .ReturnsAsync(entity);

        _currentUserServiceMock.Setup(u => u.Username)
            .Returns(username)
            .Verifiable();

        await _sut.Handle(command, CancellationToken.None);
    }

    [Test]
    public async Task Should_Call_Repository_Get()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new DeleteExample.Command(entity.Id);

        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(s => s == entity.Id)))
            .ReturnsAsync(entity)
            .Verifiable();

        _currentUserServiceMock.Setup(u => u.Username)
            .Returns(username);

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }

    [Test]
    public async Task Should_Return_Error_When_CurrentUser_Not_Entity_CreatedBy()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var currentUsername = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new DeleteExample.Command(entity.Id);

        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(s => s == entity.Id)))
            .ReturnsAsync(entity);

        _currentUserServiceMock.Setup(u => u.Username)
            .Returns(currentUsername);

        var actual = await _sut.Handle(command, CancellationToken.None);

        actual.Errors.Should().NotBeNullOrEmpty();
        actual.FirstError.Code.Should().Be(Errors.User.Unauthorized.Code);
    }

    [Test]
    public async Task Should_Call_Repository_Delete()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new DeleteExample.Command(entity.Id);

        _currentUserServiceMock.Setup(u => u.Username)
            .Returns(username);

        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(s => s == entity.Id)))
            .ReturnsAsync(entity);

        _repositoryMock.Setup(r => r.Delete(
                It.Is<ExampleDomainEntity>(e =>
                    e.Id == entity.Id &&
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .Verifiable();

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }
}