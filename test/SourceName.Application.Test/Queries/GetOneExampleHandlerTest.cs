using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Contracts;
using SourceName.Application.Queries;
using SourceName.Domain;

namespace SourceName.Application.Test.Queries;

[TestFixture]
public class GetOneExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private GetOneExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _sut = new GetOneExample.Handler(_repositoryMock.Object);
    }

    [Test]
    public async Task Should_Call_Repository_Get()
    {
        var entity = CreateEntity();
        var command = new GetOneExample.Query(entity.Id);

        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(s => s == entity.Id)))
            .ReturnsAsync(entity)
            .Verifiable();

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }

    [Test]
    public async Task Should_Return_Result()
    {
        var entity = CreateEntity();
        var command = new GetOneExample.Query(entity.Id);

        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(s => s == entity.Id)))
            .ReturnsAsync(entity);

        var actual = await _sut.Handle(command, CancellationToken.None);

        actual.Should().NotBeNull()
            .And.BeEquivalentTo(new GetOneExample.Response
            {
                Id = entity.Id,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
            });
    }

    private ExampleDomainEntity CreateEntity()
    {
        var description = _fixture.Create<string>();
        var createdBy = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, createdBy);
        return entity;
    }
}