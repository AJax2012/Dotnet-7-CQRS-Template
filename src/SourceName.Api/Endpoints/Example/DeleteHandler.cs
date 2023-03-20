using FluentValidation;

using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Commands;

namespace SourceName.Api.Endpoints.Example;

public static class DeleteHandler
{
    public static async Task<IResult> HandleAsync(
        DeleteExample.Command command,
        IValidator<DeleteExample.Command> validator,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await mediator.Send(command, cancellationToken);
        return result.MatchFirst(
            _ => Results.NoContent(),
            ErrorHandlingService.HandleErrors);
    }
}