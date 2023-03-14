// using AutoFixture;
// using FluentAssertions;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using NUnit.Framework;
// using SourceName.Api.Endpoints;
// using SourceName.Api.Endpoints.Example;
// using SourceName.Application.Commands;
//
// namespace SourceName.Api.Test.Endpoints;
//
// [TestFixture]
// public class ExampleUpdateEndpointTest
// {
//     private Fixture _fixture = null!;
//     private Mock<IMediator> _mediatorMock = null!;
//     private UpdateHandler _sut = null!;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _fixture = new Fixture();
//         _mediatorMock = new Mock<IMediator>();
//         _sut = new UpdateHandler(_mediatorMock.Object);
//     }
//
//     [Test]
//     public async Task Handle_Should_Call_Mediator_Send()
//     {
//         var command = _fixture.Create<UpdateExample.Command>();
//         var response = _fixture.Create<UpdateExample.Response>();
//
//         _mediatorMock.Setup(
//                 m => m.Send(
//                     It.Is<UpdateExample.Command>(c => c.Id == command.Id),
//                     It.IsAny<CancellationToken>()))
//             .ReturnsAsync(response)
//             .Verifiable();
//
//         await _sut.HandleAsync(command);
//
//         _mediatorMock.Verify();
//     }
//
//     [Test]
//     public async Task Handle_Should_Return_201_With_Response()
//     {
//         var command = _fixture.Create<UpdateExample.Command>();
//         var response = _fixture.Create<UpdateExample.Response>();
//
//         _mediatorMock.Setup(
//                 m => m.Send(
//                     It.Is<UpdateExample.Command>(c =>
//                         c.Id == command.Id &&
//                         c.Description == command.Description),
//                     It.IsAny<CancellationToken>()))
//             .ReturnsAsync(response);
//
//         var actual = await _sut.HandleAsync(command);
//
//         actual.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
//
//         var okObjectResult = actual.Result as OkObjectResult;
//
//         okObjectResult.Should().NotBeNull();
//         okObjectResult!.Value.Should().NotBeNull()
//             .And.BeSameAs(response);
//     }
// }