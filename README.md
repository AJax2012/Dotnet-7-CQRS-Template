# Dotnet 8 CQRS Template

This `dotnet new` template is meant to be minimal, but opinionated, boilerplate for a C# API template. The patterns are CQRS, Mediator, and REPR (Minimal API Endpoints).

## CQRS Onion Architecture Overview

The basic idea of CQRS in simple terms is a loosely coupled architecture. The layers do the following:

- Domain: processes business logic and ensures objects are valid
- Application: maps/translates external requests (API requests) into domain models and calls the infrastructure layer's persistence services to persist data.
- Infrastructure: manages external dependencies, such as database connections, requests to other services (e.g. API requests to other services, such as an email API), etc.
- Presentation: interacts with inbound requests to the service. In this case, a Dotnet 8 Minimal API.

<div align="center">
  <img alt="CQRS Onion" src="https://miro.medium.com/v2/resize:fit:1400/1*8eY3hTiNEWffynPPLqqZmw.jpeg" width="300" />
</div>

In this template, there is also a "Contracts" project, which contains the API model objects, including requests, responses, example requests/responses for Swagger, and other objects related to the Presentation layer.

## REPR Pattern Overview

The REPR pattern is an API endpoint pattern with a single endpoint per file. The files are separated in directories by feature. In this case, there is also a file to map each feature's endpoints. The outline for this would look like the below using a Profile object as for the example:

### Example Feature Endpoints Mapping

```csharp
// Endpoints/Profile/ProfileEndpoints.cs
public class ProfileEndpoints : IFeatureEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGroup("api/Feature")
            .MapEndpoint<CreateProfile>()
            .MapEndpoint<GetProfileById>()
            .MapEndpoint<GetProfiles>()
            .MapEndpoint<UpdateProfile>()
            .MapEndpoint<DeleteProfile>();            
    }
}
```

### Example Endpoint

```csharp
// Endpoints/Profile/CreateProfile.cs
public class CreateProfile : IEndpoint
{
    public static RouteHandlerBuilder Map(IEndpointRouteBuilder app) => app
        .MapPost("/", HandleAsync)
        .WithRequestValidation<CreateProfileRequest>()
        .Accepts<CreateProfileRequest>("application/json")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict, "application/json")
        .WithName("CreateProfile");

    public async static Task<IResult> HandleAsync(
        [FromBody] CreateProfileRequest request,
        ClaimsPrincipal user,
        [FromServices] ISender mediator,
        CancellationToken cancellationToken)
    {
        // perform any desired logic here
    
        // edit to match the desired command.
        var command = new CreateProfileCommand();

        // returns a result or an error using the ErrorOr library.
        var result = await mediator.Send(command, cancellationToken);
        
        // if result is Successful, returns CreatedAtRoute with id.
        // if error, returns ProblemDetails result.
        return result.Match(
            id => Results.CreatedAtRoute("GetProfileById", new { id = id }),
            errors => errors.ToProblemDetailsResult());
    }
}
```

Notice in the Map method, there is a WithRequestValidation method. This extension method creates an EndpointFilter that uses the FluentValidation library to validate an API request. Invalid API requests will return a Validation Problem rather than throwing an exception.

For more examples, please see the [example folder](https://github.com/AJax2012/Dotnet-8-CQRS-Template/tree/main/example) for a fully generated template with Profile endpoints.

## Helpful Project

The [Dotnet CQRS REPR pattern CRUD Templates](https://github.com/AJax2012/DotnetCqrsReprCrudTemplates) repository is a helpful project to help create the boilerplate classes for this template. It auto-generates the endpoints, API request/response objects, Commands, Queries, Repository, and tests. It doesn't do much beyond creating the boilerplate files, but it does save quite a bit of time. Please view the project for more information.
