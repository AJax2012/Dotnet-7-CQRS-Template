using Ardalis.GuardClauses;
using AutoMapper;

using ErrorOr;

using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;

namespace SourceName.Application.Queries;

public static class GetAllExample
{
    public record Query : IRequest<ErrorOr<GetAllResponse>>;

    public class Handler : IRequestHandler<Query, ErrorOr<GetAllResponse>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<GetAllResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entities = await _repository.Get();

            if (entities is null)
            {
                return Errors.Entity.NotFound;
            }

            var result = _mapper.Map<IEnumerable<ExampleListItem>>(entities);
            return new GetAllResponse { Results = result };
        }
    }

    public record ExampleListItem(string Id, string Description);
    public record GetAllResponse : CqrsResponse
    {
        public IEnumerable<ExampleListItem> Results { get; init; } = new List<ExampleListItem>();
    }
}