using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Commands;

namespace SourceName.Api.Endpoints.Example;

public static class UpdateHandler
{
    public static async Task<IResult> HandleAsync(UpdateExample.UpdateCommand request, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(request, cancellationToken);

        return response.MatchFirst(Results.Ok, ErrorHandlingService.HandleErrors);
    }
}