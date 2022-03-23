using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceName.Application.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace SourceName.Api.Endpoints;

public class ExampleCreateEndpoint : EndpointBaseAsync
    .WithRequest<CreateExample.Command>
    .WithActionResult<CreateExample.Response>
{
    private readonly IMediator _mediator;

    public ExampleCreateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("api/example")]
    [SwaggerOperation(
        Summary = "Create example",
        Description = "Create single example",
        OperationId = "Example.Create",
        Tags = new []{ "ExampleEndpoint" }
    )]
    public override async Task<ActionResult<CreateExample.Response>> HandleAsync(CreateExample.Command request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return CreatedAtRoute("GetExample", new { id = response.Id }, response);
    }
}