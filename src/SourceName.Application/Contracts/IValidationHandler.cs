using SourceName.Application.Common.Dtos;

namespace SourceName.Application.Contracts;

public interface IValidationHandler
{
}

public interface IValidationHandler<TRequest> : IValidationHandler
{
    Task<ValidationResult> Validate(TRequest request);
}