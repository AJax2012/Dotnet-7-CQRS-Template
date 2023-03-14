using MediatR;
using Microsoft.AspNetCore.Mvc;
using SourceName.Api.Services;
using SourceName.Application.Commands;

namespace SourceName.Api.Endpoints.Example;

public static class CreateHandler
{
    public static async Task<IResult> HandleAsync(
        [FromBody] CreateExample.CreateCommand request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(request, cancellationToken);

        return response.MatchFirst(
            entity => Results.CreatedAtRoute("GetExample", new { id = entity.Id }, entity),
            ErrorHandlingService.HandleErrors);
    }
}