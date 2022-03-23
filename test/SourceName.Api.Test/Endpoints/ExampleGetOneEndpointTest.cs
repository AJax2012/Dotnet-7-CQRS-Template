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
using SourceName.Application.Queries;

namespace SourceName.Api.Test.Endpoints;

[TestFixture]
public class ExampleGetOneEndpointTest
{
    private Fixture _fixture = null!;
    private Mock<IMediator> _mediatorMock = null!;
    private ExampleGetOneEndpoint _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _sut = new ExampleGetOneEndpoint(_mediatorMock.Object);
    }

    [Test]
    public async Task Handle_Should_Call_Mediator_Send()
    {
        var id = _fixture.Create<string>();
        var response = _fixture.Create<GetOneExample.Response>();
        
        _mediatorMock.Setup(m =>
                m.Send(It.Is<GetOneExample.Query>(q => q.Id == id), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response)
            .Verifiable();

        await _sut.HandleAsync(id);
        
        _mediatorMock.Verify();
    }

    [Test]
    public async Task Handle_Should_Return_201_With_Response()
    {
        var id = _fixture.Create<string>();
        var response = _fixture.Build<GetOneExample.Response>()
            .With(r => r.ErrorMessage, (string) null)
            .With(r => r.StatusCode, HttpStatusCode.OK)
            .WithAutoProperties()
            .Create();
        
        _mediatorMock.Setup(m =>
                m.Send(It.IsAny<GetOneExample.Query>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await _sut.HandleAsync(id);
        
        actual.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        var result = actual.Result as OkObjectResult;

        result!.Value.Should().NotBeNull().And.BeOfType<GetOneExample.Response>();
        var value = result.Value as GetOneExample.Response;
        
        value!.Should().NotBeNull().And.BeSameAs(response);
    }
}