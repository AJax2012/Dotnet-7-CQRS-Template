using System.Net;

namespace SourceName.Application.Common.Dtos;

public record CqrsResponse
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
    public string? ErrorMessage { get; init; }
}