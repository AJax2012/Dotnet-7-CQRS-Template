using AutoFixture;
using Moq;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Application.Test.Commands;

[TestFixture]
public class DeleteExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private DeleteExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _sut = new DeleteExample.Handler(_repositoryMock.Object);
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

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }

    [Test]
    public async Task Should_Call_Repository_Delete()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new DeleteExample.Command(entity.Id);

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