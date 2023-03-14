using ErrorOr;
using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;

namespace SourceName.Application.Commands;

public static class UpdateExample
{
    public record UpdateCommand(string Id, string Description) : IRequest<ErrorOr<UpdateResponse>>;

    public class Handler : IRequestHandler<UpdateCommand, ErrorOr<UpdateResponse>>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<UpdateResponse>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserService.Username ?? "test";

            if (string.IsNullOrWhiteSpace(currentUser))
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

            entity.Update(request.Description, currentUser);
            var response = await _repository.Update(entity);

            if (response is null)
            {
                return Errors.Entity.UpdateError;
            }

            return new UpdateResponse
            {
                Id = response.Id,
                Description = response.Description,
                CreatedDate = response.CreatedDate,
                UpdatedDate = response.UpdatedDate,
            };
        }
    }

    public record UpdateResponse : CqrsResponse
    {
        public string Id { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}