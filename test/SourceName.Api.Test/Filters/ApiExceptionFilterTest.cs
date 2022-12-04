using Ardalis.GuardClauses;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using Serilog;
using SourceName.Api.Filters;

namespace SourceName.Api.Test.Filters;

[TestFixture]
public class ApiExceptionFilterTest
{
    private Fixture _fixture = null!;
    private Mock<ILogger> _logger = null!;
    private ApiExceptionFilter _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _logger = new Mock<ILogger>();
        _sut = new ApiExceptionFilter(_logger.Object);
    }

    [Test]
    public void OnException_Should_Handle_NotFound_Exception()
    {
        var actionContext = _fixture.Build<ActionContext>()
            .With(ac => ac.ActionDescriptor, new ActionDescriptor())
            .With(ac => ac.HttpContext, new DefaultHttpContext())
            .WithAutoProperties()
            .Create();

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = _fixture.Create<NotFoundException>(),
            HttpContext = new DefaultHttpContext(),
        };

        _sut.OnException(context);

        context.Result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
        context.ExceptionHandled.Should().BeTrue();
    }

    [Test]
    public void OnException_Should_Handle_UnauthorizedAccessException_Exception()
    {
        var actionContext = _fixture.Build<ActionContext>()
            .With(ac => ac.ActionDescriptor, new ActionDescriptor())
            .With(ac => ac.HttpContext, new DefaultHttpContext())
            .WithAutoProperties()
            .Create();

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = _fixture.Create<UnauthorizedAccessException>(),
            HttpContext = new DefaultHttpContext(),
        };

        _sut.OnException(context);

        context.Result.Should().NotBeNull().And.BeOfType<UnauthorizedResult>();
        context.ExceptionHandled.Should().BeTrue();
    }

    [Test]
    public void OnException_Should_Handle_UnknownException()
    {
        var actionContext = _fixture.Build<ActionContext>()
            .With(ac => ac.ActionDescriptor, new ActionDescriptor())
            .With(ac => ac.HttpContext, new DefaultHttpContext())
            .WithAutoProperties()
            .Create();

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = _fixture.Create<TimeoutException>(),
            HttpContext = new DefaultHttpContext(),
        };

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = context.Exception.Message,
        };

        _sut.OnException(context);

        context.ExceptionHandled.Should().BeTrue();

        context.Result.Should().NotBeNull().And.BeOfType<ObjectResult>();
        var result = context.Result as ObjectResult;

        result!.StatusCode.Should().Be(500);
        result.Value.Should().BeEquivalentTo(details);
    }
}