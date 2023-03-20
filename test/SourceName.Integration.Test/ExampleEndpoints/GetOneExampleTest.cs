using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Queries;

namespace SourceName.Integration.Test.ExampleEndpoints;

[TestFixture]
public class GetOneExampleTest
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
    public async Task Should_Return_Ok_When_Valid()
    {
        var createCommand = _fixture.Create<CreateExample.CreateCommand>();
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, createCommand);
        var createBody = await createResponse.Content.ReadFromJsonAsync<CreateExample.CreatedResponse>();

        var response = await _client.GetAsync(Path.Combine(BaseUrl, createBody!.Id));
        var body = await response.Content.ReadFromJsonAsync<GetOneExample.GetOneResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Description.Should().Be(createCommand.Description);
    }

    [Test]
    public async Task Should_Return_BadRequest_When_Id_Invalid_Length()
    {
        var id = _fixture.Create<Guid>().ToString()[..10];

        var expectedErrors = new Dictionary<string, string[]>
            { { "Id", new[] { "'Id' must be 36 characters in length. You entered 10 characters." } } };

        var response = await _client.GetAsync(Path.Combine(BaseUrl, id));
        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Errors.Should().BeEquivalentTo(expectedErrors);
        body.Status.Should().Be((int)HttpStatusCode.BadRequest);
        body.Title.Should().Be("One or more validation errors occurred.");
        body.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
    }

    [Test]
    public async Task Should_Return_NotFound_When_Example_Does_Not_Exist()
    {
        var id = _fixture.Create<Guid>().ToString();
        var response = await _client.GetAsync(Path.Combine(BaseUrl, id));
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        body!.Detail.Should().Be("Could not find Entity");
        body.Status.Should().Be((int)HttpStatusCode.NotFound);
        body.Title.Should().Be("An error occurred while processing your request.");
        body.Type.Should().Be("https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4");
    }
}