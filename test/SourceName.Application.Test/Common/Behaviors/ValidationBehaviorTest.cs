using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using Serilog;
using SourceName.Application.Common.Behaviors;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;
using SourceName.Application.Test.TestUtils;

namespace SourceName.Application.Test.Common.Behaviors;

[TestFixture]
public class ValidationBehaviorTest
{
    private Fixture _fixture = null!;
    private Mock<ILogger> _loggerMock = null!;
    private Mock<IValidationHandler<TestHelper.Query>> _validationHandlerMock = null!;
    private Mock<RequestHandlerDelegate<TestHelper.Response>> _requestHandlerDelegateMock = null!;
    private TestHelper.Query _query = null!;
    private ValidationBehavior<TestHelper.Query, TestHelper.Response> _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _loggerMock = new Mock<ILogger>();
        _validationHandlerMock = new Mock<IValidationHandler<TestHelper.Query>>();
        _requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<TestHelper.Response>>();
        _query = new TestHelper.Query();
        _sut = new ValidationBehavior<TestHelper.Query, TestHelper.Response>(_loggerMock.Object, _validationHandlerMock.Object);
    }

    [Test]
    public async Task Should_Log_Information_When_No_Validation_Handler()
    {
        var sut = new ValidationBehavior<TestHelper.Query, TestHelper.Response>(_loggerMock.Object);
        await sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        _loggerMock.Verify(
            l => l.Information(
                It.Is<string>(s => s == "{Request} does not have a validation handler configured"),
                It.Is<string>(s => s == "Query")),
            Times.Once);
    }

    [Test]
    public async Task Should_Call_Validator()
    {
        _validationHandlerMock.Setup(v =>
                v.Validate(It.Is<TestHelper.Query>(q => q == _query)))
            .ReturnsAsync(ValidationResult.Success)
            .Verifiable();

        await _sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        _validationHandlerMock.Verify();
    }

    [Test]
    public async Task Should_Log_Warning_When_Validation_Unsuccessful()
    {
        var error = _fixture.Create<string>();

        _validationHandlerMock.Setup(v =>
                v.Validate(It.Is<TestHelper.Query>(q => q == _query)))
            .ReturnsAsync(ValidationResult.Fail(error));

        await _sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        _loggerMock.Verify(
            l => l.Warning(
            It.Is<string>(s => s == "Validation failed for {Request}. Error: {Error}"),
            It.Is<string>(s => s == "Query"),
            It.Is<string>(s => s == error)),
            Times.Once);
    }

    [Test]
    public async Task Should_Return_BadRequest_With_Error_When_Validation_Unsuccessful()
    {
        var error = _fixture.Create<string>();

        _validationHandlerMock.Setup(v =>
                v.Validate(It.Is<TestHelper.Query>(q => q == _query)))
            .ReturnsAsync(ValidationResult.Fail(error));

        var actual = await _sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        actual.Should().NotBeNull().And.BeOfType<TestHelper.Response>();
        actual.ErrorMessage.Should().NotBeNull().And.Be(error);
        actual.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_Log_Information_When_Validation_Successful()
    {
        _validationHandlerMock.Setup(v =>
                v.Validate(It.Is<TestHelper.Query>(q => q == _query)))
            .ReturnsAsync(ValidationResult.Success);

        await _sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        _loggerMock.Verify(
            l => l.Information(
            It.Is<string>(s => s == "Validation successful for {Request}"),
            It.Is<string>(s => s == "Query")),
            Times.Once);
    }

    [Test]
    public async Task Should_Return_Null_When_Validation_Successful()
    {
        _validationHandlerMock.Setup(v =>
                v.Validate(It.Is<TestHelper.Query>(q => q == _query)))
            .ReturnsAsync(ValidationResult.Success);

        var actual = await _sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        actual.Should().BeNull();
    }
}