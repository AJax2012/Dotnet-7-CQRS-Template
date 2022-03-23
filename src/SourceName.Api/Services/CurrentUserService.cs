using SourceName.Application.Contracts;

namespace SourceName.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        Username = httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }

    public string? Username { get; }
}