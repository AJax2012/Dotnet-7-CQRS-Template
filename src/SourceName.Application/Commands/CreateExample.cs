using ErrorOr;
using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;
using SourceName.Domain;

using ValidationResult = SourceName.Application.Common.Dtos.ValidationResult;

namespace SourceName.Application.Commands;

public static class CreateExample
{
    public record CreateCommand(string Description) : IRequest<ErrorOr<CreatedResponse>>;

    public class Validator : IValidationHandler<CreateCommand>
    {
        private readonly IRepository _repository;

        public Validator(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ValidationResult> Validate(CreateCommand request)
        {
            var result = await _repository.GetByDescription(request.Description);
            return result is not null ? ValidationResult.Fail("Description already exists") : ValidationResult.Success;
        }
    }

    public class Handler : IRequestHandler<CreateCommand, ErrorOr<CreatedResponse>>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<CreatedResponse>> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            // null check for example purposes only - cannot make JWT with this example
            var currentUser = _currentUserService.Username ?? "test";

            if (currentUser is null)
            {
                return Errors.User.NotFound;
            }

            var entity = new ExampleDomainEntity();
            entity.Create(request.Description, currentUser);

            var response = await _repository.Create(entity);

            if (response is null)
            {
                return Errors.Entity.CreateError;
            }

            return new CreatedResponse { Id = response.Id, Description = response.Description };
        }
    }

    public record CreatedResponse : CqrsResponse
    {
        public string Id { get; init; } = null!;
        public string Description { get; init; } = null!;
    }
}