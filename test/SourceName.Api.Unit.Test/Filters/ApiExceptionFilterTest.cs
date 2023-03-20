using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using Moq;

using NUnit.Framework;

using Serilog;

using SourceName.Api.Filters;

namespace SourceName.Api.Unit.Test.Filters;

[TestFixture]
public class ApiExceptionFilterTest
{
    private HttpContext _context = null!;
    private Mock<ILogger> _loggerMock = null!;
    private Mock<IHostEnvironment> _hostEnvironmentMock = null!;
    private ApiExceptionMiddleware _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _context = new DefaultHttpContext();
        _loggerMock = new Mock<ILogger>();
        _hostEnvironmentMock = new Mock<IHostEnvironment>();
    }

    [Test]
    public async Task InvokeAsync_Should_Handle_UnauthorizedAccessException_Exception()
    {
        var next = new RequestDelegate(_ => Task.FromException<UnauthorizedAccessException>(new UnauthorizedAccessException()));
        _sut = new ApiExceptionMiddleware(next, _loggerMock.Object, _hostEnvironmentMock.Object);

        await _sut.InvokeAsync(_context);

        _context.Should().NotBeNull();
        _context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        _context.Response.ContentType.Should().Be("application/json");
    }

    [Test]
    public async Task InvokeAsync_Should_Handle_UnknownException()
    {
        var next = new RequestDelegate(_ => Task.FromException<Exception>(new Exception()));
        _sut = new ApiExceptionMiddleware(next, _loggerMock.Object, _hostEnvironmentMock.Object);

        await _sut.InvokeAsync(_context);

        _context.Should().NotBeNull();
        _context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        _context.Response.ContentType.Should().Be("application/json");
    }
}