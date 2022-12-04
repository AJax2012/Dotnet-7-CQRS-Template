using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceName.Application.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace SourceName.Api.Endpoints;

public class ExampleGetAllEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<GetAllExample.Response>
{
    private readonly IMediator _mediator;

    public ExampleGetAllEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("api/example")]
    [SwaggerOperation(
        Summary = "Get Example List",
        Description = "Get all examples",
        OperationId = "Example.GetAll",
        Tags = new[] { "ExampleEndpoint" })]
    public override async Task<ActionResult<GetAllExample.Response>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllExample.Query(), cancellationToken);
        return Ok(result);
    }
}