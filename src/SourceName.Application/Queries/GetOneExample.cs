using ErrorOr;
using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;

namespace SourceName.Application.Queries;

public static class GetOneExample
{
    public record Query(string Id) : IRequest<ErrorOr<GetOneResponse>>;

    public class Handler : IRequestHandler<Query, ErrorOr<GetOneResponse>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ErrorOr<GetOneResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(request.Id);

            if (entity is null)
            {
                return Errors.Entity.NotFound;
            }

            return new GetOneResponse { Id = entity.Id, Description = entity.Description, CreatedDate = entity.CreatedDate, UpdatedDate = entity.UpdatedDate };
        }
    }

    public record GetOneResponse : CqrsResponse
    {
        public string Id { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}