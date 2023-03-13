using Ardalis.GuardClauses;
using ErrorOr;
using MediatR;

using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;

namespace SourceName.Application.Commands;

public static class DeleteExample
{
    public record Command(string Id) : IRequest<ErrorOr<Deleted>>;

    public class Handler : IRequestHandler<Command, ErrorOr<Deleted>>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ErrorOr<Deleted>> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(request.Id);

            if (entity is null)
            {
                return Errors.Entity.NotFound;
            }

            await _repository.Delete(entity);
            return Result.Deleted;
        }
    }
}