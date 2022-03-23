using SourceName.SharedKernel;

namespace SourceName.Domain;

public class ExampleDomainEntity
{
    public string Id { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }

    public void Create(string description, string createdBy)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        CreatedBy = createdBy;
        CreatedDate = DateTimeProvider.Now;
        UpdatedDate = DateTimeProvider.Now;
    }

    public void Update(string description, string updatedBy)
    {
        Description = description;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTimeProvider.Now;
    }
}