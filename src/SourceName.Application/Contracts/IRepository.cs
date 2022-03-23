using SourceName.Domain;

namespace SourceName.Application.Contracts;

public interface IRepository
{
    Task<ExampleDomainEntity> Get(string id);
    Task<IEnumerable<ExampleDomainEntity>> Get();
    Task<ExampleDomainEntity> Create(ExampleDomainEntity entity);
    Task<ExampleDomainEntity> Update(ExampleDomainEntity entity);
    Task Delete(ExampleDomainEntity entity);
    Task<ExampleDomainEntity> GetByDescription(string description);
}