using Ardalis.GuardClauses;
using MediatR;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;

namespace SourceName.Application.Commands;

public static class UpdateExample
{
    public record Command(string Id, string Description) : IRequest<Response>;

    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IRepository repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }
        
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Get(request.Id);
            var currentUser  = _currentUserService.Username ?? "test";
            
            Guard.Against.Null(entity);
            Guard.Against.NullOrWhiteSpace(currentUser, "CurrentUser");
            
            entity.Update(request.Description, currentUser);
            var response = await _repository.Update(entity);
            return new Response
            {
                Id = response.Id,
                Description = response.Description,
                CreatedDate = response.CreatedDate,
                UpdatedDate = response.UpdatedDate,
            };
        }
    }

    public record Response : CqrsResponse
    {
        public string Id { get; init; }
        public string Description { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
    }
}