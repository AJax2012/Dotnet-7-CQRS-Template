using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SourceName.Api.Endpoints;
using SourceName.Application.Commands;

namespace SourceName.Api.Test.Endpoints;

[TestFixture]
public class ExampleDeleteEndpointTest
{
    private Fixture _fixture = null!;
    private Mock<IMediator> _mediatorMock = null!;
    private ExampleDeleteEndpoint _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _sut = new ExampleDeleteEndpoint(_mediatorMock.Object);
    }

    [Test]
    public async Task Handle_Should_Call_Mediator_Send()
    {
        var id = _fixture.Create<string>();
        var command = _fixture.Build<DeleteExample.Command>()
            .With(r => r.Id, id)
            .Create();

        _mediatorMock.Setup(m =>
                m.Send(
                    It.Is<DeleteExample.Command>(c => c.Id == command.Id),
                    It.IsAny<CancellationToken>()))
            .Verifiable();

        await _sut.HandleAsync(id);

        _mediatorMock.Verify();
    }

    [Test]
    public async Task Handle_Should_Return_201_With_Response()
    {
        var id = _fixture.Create<string>();
        var command = _fixture.Build<DeleteExample.Command>()
            .With(r => r.Id, id)
            .Create();

        _mediatorMock.Setup(
            m => m.Send(
                It.Is<DeleteExample.Command>(c => c.Id == command.Id),
                It.IsAny<CancellationToken>()));

        var actual = await _sut.HandleAsync(id);

        actual.Should().NotBeNull().And.BeOfType<OkObjectResult>();

        var okObjectResult = actual as OkObjectResult;

        okObjectResult.Should().NotBeNull();
        okObjectResult!.Value.Should().NotBeNull()
            .And.BeEquivalentTo(new { id = id });
    }
}