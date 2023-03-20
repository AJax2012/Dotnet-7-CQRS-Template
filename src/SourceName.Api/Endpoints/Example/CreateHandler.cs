using FluentValidation;
using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Commands;

namespace SourceName.Api.Endpoints.Example;

public static class CreateHandler
{
    public static async Task<IResult> HandleAsync(
        CreateExample.CreateCommand request,
        IValidator<CreateExample.CreateCommand> validator,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await mediator.Send(request, cancellationToken);

        return response.MatchFirst(
            entity => Results.CreatedAtRoute("GetExample", new { id = entity.Id }, entity),
            ErrorHandlingService.HandleErrors);
    }
}