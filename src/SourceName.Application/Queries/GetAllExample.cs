using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;

namespace SourceName.Application.Queries;

public static class GetAllExample
{
    public record Query() : IRequest<Response>;
    
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entities = await _repository.Get();
            Guard.Against.Null(entities);
            var result = _mapper.Map<IEnumerable<ExampleListItem>>(entities);
            return new Response{ Results = result };
        }
    }

    public record ExampleListItem(string Id, string Description);
    public record Response : CqrsResponse
    {
        public IEnumerable<ExampleListItem> Results { get; init; }
    }
}