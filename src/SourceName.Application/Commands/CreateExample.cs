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
    public record Command(string Description) : IRequest<ErrorOr<Response>>;

    public class Validator : IValidationHandler<Command>
    {
        private readonly IRepository _repository;

        public Validator(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ValidationResult> Validate(Command request)
        {
            var result = await _repository.GetByDescription(request.Description);
            return result is not null ? ValidationResult.Fail("Description already exists") : ValidationResult.Success;
        }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<Response>>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            // null check for example purposes only - cannot make JWT with this example
            var currentUser = _currentUserService.Username;

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

            return new Response { Id = response.Id, Description = response.Description };
        }
    }

    public record Response : CqrsResponse
    {
        public string Id { get; init; } = null!;
        public string Description { get; init; } = null!;
    }
}