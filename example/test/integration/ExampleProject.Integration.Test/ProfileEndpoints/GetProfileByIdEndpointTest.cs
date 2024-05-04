using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

using ExampleProject.Integration.Test;

namespace ExampleProject.Integration.Test.ProfileEndpoints;

/// <summary>
/// Integration tests for creating a Profile.
/// </summary>
public class GetProfileByIdEndpointTest : IClassFixture<ExampleProjectApiFactory>
{
    private const string BaseUrl = "api/profiles";
    private readonly HttpClient _client;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="GetProfileByIdEndpointTest"/> class.
	/// </summary>
	/// <param name="apiFactory"></param>
	public GetProfileByIdEndpointTest(ExampleProjectApiFactory apiFactory)
	{
		_client = apiFactory.CreateClient();
	}
}