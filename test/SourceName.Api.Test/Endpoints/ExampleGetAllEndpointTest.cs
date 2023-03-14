// using System.Net;
// using AutoFixture;
// using FluentAssertions;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using NUnit.Framework;
// using SourceName.Api.Endpoints;
// using SourceName.Api.Endpoints.Example;
// using SourceName.Application.Queries;
//
// namespace SourceName.Api.Test.Endpoints;
//
// [TestFixture]
// public class ExampleGetAllEndpointTest
// {
//     private Fixture _fixture = null!;
//     private Mock<IMediator> _mediatorMock = null!;
//     private GetAllHandler _sut = null!;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _fixture = new Fixture();
//         _mediatorMock = new Mock<IMediator>();
//         _sut = new GetAllHandler(_mediatorMock.Object);
//     }
//
//     [Test]
//     public async Task Handle_Should_Call_Mediator_Send()
//     {
//         var response = _fixture.Create<GetAllExample.Response>();
//
//         _mediatorMock.Setup(
//                 m => m.Send(
//                     It.IsAny<GetAllExample.Query>(),
//                     It.IsAny<CancellationToken>()))
//             .ReturnsAsync(response)
//             .Verifiable();
//
//         await _sut.HandleAsync();
//
//         _mediatorMock.Verify();
//     }
//
//     [Test]
//     public async Task Handle_Should_Return_201_With_Response()
//     {
//         var response = _fixture.Build<GetAllExample.Response>()
//             .With(r => r.ErrorMessage, (string?)null)
//             .With(r => r.StatusCode, HttpStatusCode.OK)
//             .With(r => r.Results, _fixture.CreateMany<GetAllExample.ExampleListItem>(5))
//             .WithAutoProperties()
//             .Create();
//
//         _mediatorMock.Setup(
//                 m => m.Send(
//                     It.IsAny<GetAllExample.Query>(),
//                     It.IsAny<CancellationToken>()))
//             .ReturnsAsync(response);
//
//         var actual = await _sut.HandleAsync();
//
//         actual.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
//         var result = actual.Result as OkObjectResult;
//
//         result!.Value.Should().NotBeNull().And.BeOfType<GetAllExample.Response>();
//         var value = result.Value as GetAllExample.Response;
//
//         value!.Results.Should().NotBeNull()
//             .And.HaveSameCount(response.Results)
//             .And.BeSameAs(response.Results);
//     }
// }