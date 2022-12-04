using System.Threading;
using MediatR;
using Moq;
using NUnit.Framework;
using Serilog;
using SourceName.Application.Common.Behaviors;
using SourceName.Application.Test.TestUtils;

namespace SourceName.Application.Test.Common.Behaviors;

[TestFixture]
public class LoggingBehaviorTest
{
    private Mock<ILogger> _loggerMock = null!;
    private Mock<RequestHandlerDelegate<TestHelper.Response>> _requestHandlerDelegateMock = null!;
    private TestHelper.Query _query = null!;
    private LoggingBehavior<TestHelper.Query, TestHelper.Response> _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger>();
        _requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<TestHelper.Response>>();
        _query = new TestHelper.Query();
        _sut = new LoggingBehavior<TestHelper.Query, TestHelper.Response>(_loggerMock.Object);
    }

    [Test]
    public void Should_Log_Information()
    {
        _sut.Handle(_query, CancellationToken.None, _requestHandlerDelegateMock.Object);

        _loggerMock.Verify(
            l => l.Information(
                It.Is<string>(s => s == "{Request} is starting"),
                It.Is<string>(v => v == "Query")),
            Times.Once);

        _loggerMock.Verify(
            l => l.Information(
                It.Is<string>(s => s == "{Request} has finished in {Time} ms"),
                It.Is<string>(v => v == "Query"),
                It.IsAny<long>()),
            Times.Once);
    }
}