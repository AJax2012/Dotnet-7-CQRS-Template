using System;
using System.Text;
using Newtonsoft.Json;
using ExampleProject.Application.Common.Models;

namespace ExampleProject.Application.Common.Services;

/// <summary>
/// Service for encoding and decoding next page tokens.
/// </summary>
internal static class NextPageEncodingService
{
	/// <summary>
	/// Encodes the next page object to a base64 string.
	/// </summary>
	/// <param name="nextPage"></param>
	/// <returns></returns>
	internal static string EncodeNextPageToken<T>(this T nextPage) where T : NextPageDto
	{
		var json = JsonConvert.SerializeObject(nextPage);
		var bytes = Encoding.Default.GetBytes(json);
		return Convert.ToBase64String(bytes);
	}

	/// <summary>
	/// Decodes the next page token to the next page object.
	/// </summary>
	/// <param name="nextPageToken"></param>
	/// <returns></returns>
	internal static T? DecodeNextPageToken<T>(this string nextPageToken) where T : NextPageDto
	{
		var bytes = Convert.FromBase64String(nextPageToken);
		var json = Encoding.Default.GetString(bytes);
		return JsonConvert.DeserializeObject<T>(json);
	}
}