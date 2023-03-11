using Ardalis.GuardClauses;
using MediatR;
using SourceName.Application.Contracts;

namespace SourceName.Application.Commands;

public static class DeleteExample
{
    public record Command(string Id) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(request.Id);
            Guard.Against.Null(entity);
            await _repository.Delete(entity);
        }
    }
}