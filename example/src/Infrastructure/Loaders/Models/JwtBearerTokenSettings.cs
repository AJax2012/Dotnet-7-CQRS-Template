namespace ExampleProject.Infrastructure.Loaders.Models;

public class JwtBearerTokenSettings
{
    public string SecretKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiryTimeInSeconds { get; init; }
}