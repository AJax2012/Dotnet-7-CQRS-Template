using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Queries;

namespace SourceName.Api.Endpoints.Example;

public static class GetOneHandler
{
    public static async Task<IResult> HandleAsync(GetOneExample.Query query, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(query, cancellationToken);

        return response.MatchFirst(Results.Ok, ErrorHandlingService.HandleErrors);
    }
}