using Ardalis.ApiEndpoints;
using SourceName.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace SourceName.Api.Endpoints;

public class ExampleGetOneEndpoint : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<GetOneExample.Response>
{
    private readonly IMediator _mediator;

    public ExampleGetOneEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("api/example/{id}", Name = "GetExample")]
    [SwaggerOperation(
        Summary = "Get example",
        Description = "Get example by id",
        OperationId = "Example.GetOne",
        Tags = new []{ "ExampleEndpoint" }
    )]
    public override async Task<ActionResult<GetOneExample.Response>> HandleAsync([FromRoute] string id, CancellationToken cancellationToken = new CancellationToken())
    {
        var response = await _mediator.Send(new GetOneExample.Query(id), cancellationToken);
        return Ok(response);
    }
}