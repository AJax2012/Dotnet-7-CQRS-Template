using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Commands;

namespace SourceName.Api.Endpoints.Example;

public static class DeleteHandler
{
    public static async Task<IResult> HandleAsync(
        DeleteExample.Command command,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.MatchFirst(
            _ => Results.NoContent(),
            ErrorHandlingService.HandleErrors);
    }
}