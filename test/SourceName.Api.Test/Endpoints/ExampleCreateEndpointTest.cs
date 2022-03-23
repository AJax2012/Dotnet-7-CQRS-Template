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
public class ExampleCreateEndpointTest
{
    private Fixture _fixture = null!;
    private Mock<IMediator> _mediatorMock = null!;
    private ExampleCreateEndpoint _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _sut = new ExampleCreateEndpoint(_mediatorMock.Object);
    }

    [Test]
    public async Task Handle_Should_Call_Mediator_Send()
    {
        var command = _fixture.Create<CreateExample.Command>();
        var response = _fixture.Create<CreateExample.Response>();
        
        _mediatorMock.Setup(m =>
                m.Send(It.Is<CreateExample.Command>(c =>
                    c.Description == command.Description), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response)
            .Verifiable();

        await _sut.HandleAsync(command);
        
        _mediatorMock.Verify();
    }

    [Test]
    public async Task Handle_Should_Return_201_With_Response()
    {
        var command = _fixture.Create<CreateExample.Command>();
        var response = _fixture.Build<CreateExample.Response>()
            .With(r => r.ErrorMessage, (string) null)
            .With(r => r.StatusCode, HttpStatusCode.OK)
            .With(r => r.Description, command.Description)
            .WithAutoProperties()
            .Create();

        _mediatorMock.Setup(m =>
                m.Send(It.Is<CreateExample.Command>(c =>
                    c.Description == command.Description), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await _sut.HandleAsync(command);

        actual.Result.Should().NotBeNull().And.BeOfType<CreatedAtRouteResult>();
        
        var createdAtResult = actual.Result as CreatedAtRouteResult;

        createdAtResult.Value.Should().NotBeNull()
            .And.BeOfType<CreateExample.Response>()
            .And.BeSameAs(response);
        
        createdAtResult.RouteName.Should().Be("GetExample");
        createdAtResult.RouteValues.Should().NotBeNull();
        createdAtResult.RouteValues!.Count.Should().Be(1);
        createdAtResult.RouteValues.First().Key.Should().Be("id");
        createdAtResult.RouteValues.First().Value.Should().Be(response.Id);
    }
}