using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SourceName.Application.Commands;

namespace SourceName.Integration.Test.ExampleEndpoints;

[TestFixture]
public class CreateExampleTest
{
    private const string BaseUrl = "api/example";

    private Fixture _fixture = null!;
    private HttpClient _client = null!;
    private SourceNameApiFactory _applicationFactory = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _applicationFactory = new SourceNameApiFactory();
        _client = _applicationFactory.CreateClient();
    }

    [Test]
    public async Task Should_Return_Created_When_Valid()
    {
        var command = _fixture.Create<CreateExample.CreateCommand>();
        var response = await _client.PostAsJsonAsync(BaseUrl, command);
        var body = await response.Content.ReadFromJsonAsync<CreateExample.CreatedResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        body.Should().NotBeNull();
        body!.Description.Should().Be(command.Description);
    }

    [Test]
    public async Task Should_Return_BadRequest_When_Description_Is_Empty_String()
    {
        var expectedErrors = new Dictionary<string, string[]>
            { { "Description", new[] { "'Description' must not be empty.", "The length of 'Description' must be at least 1 characters. You entered 0 characters." } } };

        var command = _fixture.Build<CreateExample.CreateCommand>()
            .With(c => c.Description, string.Empty)
            .Create();

        var response = await _client.PostAsJsonAsync(BaseUrl, command);
        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Errors.Should().BeEquivalentTo(expectedErrors);
        body.Status.Should().Be((int)HttpStatusCode.BadRequest);
        body.Title.Should().Be("One or more validation errors occurred.");
        body.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
    }

    [TestCase(null)]
    [TestCase("   ")]
    public async Task Should_Return_BadRequest_When_Description_Is_Null_Or_Whitespace(string? description)
    {
        var expectedErrors = new Dictionary<string, string[]>
            { { "Description", new[] { "'Description' must not be empty." } } };

        var command = _fixture.Build<CreateExample.CreateCommand>()
            .With(c => c.Description, description)
            .Create();

        var response = await _client.PostAsJsonAsync(BaseUrl, command);
        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Errors.Should().BeEquivalentTo(expectedErrors);
        body.Status.Should().Be((int)HttpStatusCode.BadRequest);
        body.Title.Should().Be("One or more validation errors occurred.");
        body.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
    }

    [Test]
    public async Task Should_Return_Invalid_Error_When_Description_Exists()
    {
        var command = _fixture.Create<CreateExample.CreateCommand>();
        await _client.PostAsJsonAsync(BaseUrl, command);
        var response = await _client.PostAsJsonAsync(BaseUrl, command);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Detail.Should().Be("Entity with description already exists.");
        body.Status.Should().Be((int)HttpStatusCode.BadRequest);
        body.Title.Should().Be("An error occurred while processing your request.");
        body.Type.Should().Be("https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1");
    }
}