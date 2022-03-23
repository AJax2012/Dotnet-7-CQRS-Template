namespace SourceName.Infrastructure.Loaders.Models;

public class JwtBearerTokenSettings
{
    public string SecretKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public int ExpiryTimeInSeconds { get; init; }
}