using Ardalis.GuardClauses;
using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;

namespace SourceName.Application.Queries;

public static class GetOneExample
{
    public record Query(string Id) : IRequest<Response>;

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(request.Id);
            Guard.Against.Null(entity);
            return new Response { Id = entity.Id, Description = entity.Description, CreatedDate = entity.CreatedDate, UpdatedDate = entity.UpdatedDate };
        }
    }

    public record Response : CqrsResponse
    {
        public string Id { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}