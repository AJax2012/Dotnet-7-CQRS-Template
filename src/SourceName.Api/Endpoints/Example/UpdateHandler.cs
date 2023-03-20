using FluentValidation;
using MediatR;
using SourceName.Api.Services;
using SourceName.Application.Commands;

namespace SourceName.Api.Endpoints.Example;

public static class UpdateHandler
{
    public static async Task<IResult> HandleAsync(
        UpdateExample.UpdateCommand request,
        IValidator<UpdateExample.UpdateCommand> validator,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await mediator.Send(request, cancellationToken);

        return response.MatchFirst(Results.Ok, ErrorHandlingService.HandleErrors);
    }
}