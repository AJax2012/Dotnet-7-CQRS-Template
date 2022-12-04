using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;

namespace SourceName.Application.Test.TestUtils;

public static class TestHelper
{
    public record Query : IRequest<Response>;

    public class Validator : IValidationHandler<Query>
    {
        public Task<ValidationResult> Validate(Query request)
        {
            return Task.FromResult(ValidationResult.Success);
        }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        public Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Response { Result = "test" });
        }
    }

    public record Response : CqrsResponse
    {
        public string Result { get; set; } = null!;
    }
}