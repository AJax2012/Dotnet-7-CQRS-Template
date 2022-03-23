namespace SourceName.Application.Contracts;

public interface ICurrentUserService
{
    string? Username { get; }
}