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
        private readonly ICurrentUserService _currentUserService;

        public Handler(IRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Deleted>> Handle(Command request, CancellationToken cancellationToken)
        {
            // null check for example purposes only - cannot make JWT with this example
            var currentUser = _currentUserService.Username ?? "test";

            if (currentUser is null)
            {
                return Errors.User.NotFound;
            }

            var entity = await _repository.Get(request.Id);

            if (entity is null)
            {
                return Errors.Entity.NotFound;
            }

            if (entity.CreatedBy != currentUser)
            {
                return Errors.User.Unauthorized;
            }

            await _repository.Delete(entity);
            return Result.Deleted;
        }
    }
}