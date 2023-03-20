using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SourceName.Api.Endpoints.Internal;
using SourceName.Application.Commands;
using SourceName.Application.Queries;

namespace SourceName.Api.Endpoints.Example;

public class ExampleEndpoints : IEndpoint
{
    private const string BaseUrl = "api/example";
    private const string ContentType = "application/json";
    private static readonly string[] Tag = { "Example" };

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(BaseUrl, CreateHandler.HandleAsync)
            .AllowAnonymous()
            .Accepts<CreateExample.CreateCommand>(ContentType)
            .Produces<CreateExample.CreatedResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("CreateExample")
            .WithTags(Tag);

        app.MapGet(BaseUrl, GetAllHandler.HandleAsync)
            .AllowAnonymous()
            .Produces<GetAllExample.GetAllResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithName("GetExamples")
            .WithTags(Tag);

        app.MapPut(BaseUrl, UpdateHandler.HandleAsync)
            .AllowAnonymous()
            .Accepts<UpdateExample.UpdateCommand>(ContentType)
            .Produces<UpdateExample.UpdateResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("UpdateExample")
            .WithTags(Tag);

        app.MapDelete($"{BaseUrl}/{{id}}", async (
                    string id,
                    IValidator<DeleteExample.Command> validator,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                await DeleteHandler.HandleAsync(new(id), validator, mediator, cancellationToken))
            .AllowAnonymous()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithName("DeleteExample")
            .WithTags(Tag);

        app.MapGet($"{BaseUrl}/{{id}}", async (
                    string id,
                    IValidator<GetOneExample.Query> validator,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                await GetOneHandler.HandleAsync(new(id), validator, mediator, cancellationToken))
            .AllowAnonymous()
            .Produces<GetOneExample.GetOneResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithName("GetExample")
            .WithTags(Tag);
    }

    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
        });
    }
}