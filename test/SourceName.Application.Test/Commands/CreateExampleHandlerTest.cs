using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Application.Test.Commands;

[TestFixture]
public class CreateExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private Mock<ICurrentUserService> _currentUserServiceMock = null!;
    private CreateExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _sut = new CreateExample.Handler(_repositoryMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public async Task Should_Call_CurrentUserService_Username()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.Command(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _currentUserServiceMock.Setup(c => c.Username)
            .Returns(username)
            .Verifiable();

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity);

        await _sut.Handle(command, CancellationToken.None);

        _currentUserServiceMock.Verify();
    }

    [Test]
    public async Task Should_Call_Repository_Create()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.Command(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _currentUserServiceMock.Setup(c => c.Username).Returns(username);

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity)
            .Verifiable();

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }

    [Test]
    public async Task Should_Return_Response()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.Command(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _currentUserServiceMock.Setup(c => c.Username).Returns(username);

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity);

        var actual = await _sut.Handle(command, CancellationToken.None);

        actual.Should().NotBeNull().And.BeOfType<CreateExample.Response>();
        actual.Id.Should().NotBeEmpty();
        actual.Description.Should().Be(description);
    }
}