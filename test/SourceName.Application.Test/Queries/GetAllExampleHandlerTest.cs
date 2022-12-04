using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Contracts;
using SourceName.Application.Queries;
using SourceName.Domain;

namespace SourceName.Application.Test.Queries;

[TestFixture]
public class GetAllExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private GetAllExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();
        _sut = new GetAllExample.Handler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task Should_Call_Repository_Get()
    {
        var entities = _fixture.CreateMany<ExampleDomainEntity>(10);
        var result = _fixture.CreateMany<GetAllExample.ExampleListItem>(10);
        var command = new GetAllExample.Query();

        _repositoryMock.Setup(r => r.Get())
            .ReturnsAsync(entities)
            .Verifiable();

        _mapperMock.Setup(m =>
                m.Map<IEnumerable<GetAllExample.ExampleListItem>>(
                    It.Is<IEnumerable<ExampleDomainEntity>>(e =>
                        e.Count() == entities.Count())))
            .Returns(result);

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }

    [Test]
    public async Task Should_Call_Mapper_Map()
    {
        var entities = _fixture.CreateMany<ExampleDomainEntity>(10);
        var result = _fixture.CreateMany<GetAllExample.ExampleListItem>(10);
        var command = new GetAllExample.Query();

        _repositoryMock.Setup(r => r.Get())
            .ReturnsAsync(entities);

        _mapperMock.Setup(m =>
                m.Map<IEnumerable<GetAllExample.ExampleListItem>>(
                    It.Is<IEnumerable<ExampleDomainEntity>>(e =>
                        e.Count() == entities.Count())))
            .Returns(result)
            .Verifiable();

        await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Verify();
    }

    [Test]
    public async Task Should_Return_Result()
    {
        var entities = _fixture.CreateMany<ExampleDomainEntity>(10);
        var expectedResponse = _fixture.CreateMany<GetAllExample.ExampleListItem>(10);
        var command = new GetAllExample.Query();

        _repositoryMock.Setup(r => r.Get())
            .ReturnsAsync(entities);

        _mapperMock.Setup(m =>
                m.Map<IEnumerable<GetAllExample.ExampleListItem>>(
                    It.Is<IEnumerable<ExampleDomainEntity>>(e =>
                        e.Count() == entities.Count())))
            .Returns(expectedResponse);

        var actual = await _sut.Handle(command, CancellationToken.None);

        actual.Should().NotBeNull()
            .And.BeEquivalentTo(new GetAllExample.Response { Results = expectedResponse });
    }
}