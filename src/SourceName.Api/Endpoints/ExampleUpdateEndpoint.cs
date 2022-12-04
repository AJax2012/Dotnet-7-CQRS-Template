using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceName.Application.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace SourceName.Api.Endpoints;

public class ExampleUpdateEndpoint : EndpointBaseAsync
    .WithRequest<UpdateExample.Command>
    .WithActionResult<UpdateExample.Response>
{
    private readonly IMediator _mediator;

    public ExampleUpdateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPut("api/example")]
    [SwaggerOperation(
        Summary = "Update example",
        Description = "Update example by id",
        OperationId = "Example.Update",
        Tags = new[] { "ExampleEndpoint" })]
    public override async Task<ActionResult<UpdateExample.Response>> HandleAsync(UpdateExample.Command request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}