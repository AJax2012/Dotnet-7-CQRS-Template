using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Queries;

namespace SourceName.Api.Endpoints.Example;

public static class GetAllHandler
{
    public static async Task<IResult> HandleAsync(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllExample.Query(), cancellationToken);
        return result.MatchFirst(Results.Ok, ErrorHandlingService.HandleErrors);
    }
}