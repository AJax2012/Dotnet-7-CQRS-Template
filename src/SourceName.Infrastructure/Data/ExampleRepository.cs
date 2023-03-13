using Microsoft.EntityFrameworkCore;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Infrastructure.Data;

public class ExampleRepository : IRepository
{
    private readonly ExampleContext _context;

    public ExampleRepository(ExampleContext context)
    {
        _context = context;
    }

    public async Task<ExampleDomainEntity?> Get(string id)
    {
        var result = await _context.Examples.FirstOrDefaultAsync(x => x.Id == id);
        return result!;
    }

    public async Task<IEnumerable<ExampleDomainEntity>?> Get()
    {
        var result = await _context.Examples.ToListAsync();
        return result;
    }

    public async Task<ExampleDomainEntity?> GetByDescription(string description)
    {
        var result = await _context.Examples.FirstOrDefaultAsync(x => x.Description == description);
        return result!;
    }

    public async Task<ExampleDomainEntity?> Create(ExampleDomainEntity entity)
    {
        var response = await _context.Examples.AddAsync(entity);
        await _context.SaveChangesAsync();
        return response.Entity;
    }

    public async Task<ExampleDomainEntity?> Update(ExampleDomainEntity entity)
    {
        _context.Examples.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(ExampleDomainEntity entity)
    {
        _context.Examples.Remove(entity);
        await _context.SaveChangesAsync();
    }
}