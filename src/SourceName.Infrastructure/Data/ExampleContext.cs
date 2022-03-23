using Microsoft.EntityFrameworkCore;
using SourceName.Domain;

namespace SourceName.Infrastructure.Data;

public class ExampleContext : DbContext
{
    public ExampleContext(DbContextOptions<ExampleContext> options) : base(options) { }

    public DbSet<ExampleDomainEntity> Examples { get; set; }
}