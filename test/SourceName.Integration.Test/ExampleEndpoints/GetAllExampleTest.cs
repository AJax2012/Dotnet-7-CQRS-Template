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
[Order(1)]
public class GetAllExampleTest
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
    [Order(2)]
    public async Task Should_Return_Ok_When_Valid()
    {
        var command1 = _fixture.Create<CreateExample.CreateCommand>();
        var command2 = _fixture.Create<CreateExample.CreateCommand>();
        await _client.PostAsJsonAsync(BaseUrl, command1);
        await _client.PostAsJsonAsync(BaseUrl, command2);

        var response = await _client.GetAsync(BaseUrl);
        var body = await response.Content.ReadFromJsonAsync<GetAllExample.GetAllResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Results.Should().NotBeNullOrEmpty().And.HaveCount(2);
        body.Results.Should().ContainSingle(r => r.Description == command1.Description)
            .And.ContainSingle(r => r.Description == command2.Description);
    }

    [Test]
    [Order(1)]
    public async Task Should_Return_NotFound_When_No_Examples()
    {
        var response = await _client.GetAsync(BaseUrl);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        body!.Detail.Should().Be("Could not find Entity");
        body.Status.Should().Be((int)HttpStatusCode.NotFound);
        body.Title.Should().Be("An error occurred while processing your request.");
        body.Type.Should().Be("https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4");
    }
}