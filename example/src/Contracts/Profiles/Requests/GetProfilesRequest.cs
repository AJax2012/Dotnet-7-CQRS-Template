using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExampleProject.Contracts.Profiles.Requests;

/// <summary>
/// Request to get a paginated list of Profiles.
/// </summary>
public record GetProfilesRequest(
	[FromQuery(Name = "limit")] int? Limit,
	[FromQuery(Name = "orderBy")] string[]? OrderBy,
	[FromQuery(Name = "isDescending")] bool? IsDescending,
	[FromQuery(Name = "next_page")] string? NextPageToken)
{
	public static bool TryParse(string? query, out GetProfilesRequest? request)
	{
		request = null;

		if (query == null)
		{
			return false;
		}

		try
		{
			request = JsonConvert.DeserializeObject<GetProfilesRequest>(query)!;
			return true;
		}
		catch
		{
			return false;
		}
	}
}