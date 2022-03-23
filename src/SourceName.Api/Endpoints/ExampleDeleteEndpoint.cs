using System.Net;
using Ardalis.ApiEndpoints;
using SourceName.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace SourceName.Api.Endpoints;

public class ExampleDeleteEndpoint : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult
{
    private readonly IMediator _mediator;

    public ExampleDeleteEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpDelete("api/example/{id}")]
    [SwaggerOperation(
        Summary = "Delete example",
        Description = "Delete example by id",
        OperationId = "Example.Delete",
        Tags = new []{ "ExampleEndpoint" }
    )]
    public override async Task<ActionResult> HandleAsync([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteExample.Command(id), cancellationToken);
        return Ok(new { id = id });
    }
}