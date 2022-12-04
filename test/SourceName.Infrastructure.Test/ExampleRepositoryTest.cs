using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SourceName.Domain;
using SourceName.Infrastructure.Data;

namespace SourceName.Infrastructure.Test;

public class ExampleRepositoryTest
{
    private Fixture _fixture = null!;
    private DbContextOptions<ExampleContext> _contextOptions = null!;
    private ExampleRepository _sut = null!;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _contextOptions = new DbContextOptionsBuilder<ExampleContext>()
            .UseInMemoryDatabase("ExampleRepositoryTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .EnableSensitiveDataLogging()
            .Options;
    }

    [Test]
    public async Task Get_Should_Get_Single_Entity()
    {
        var allData = CreateData();
        var expectedEntity = allData.First();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);

            var result = await _sut.Get(expectedEntity.Id);
            result.Should().NotBeNull().And.BeEquivalentTo(expectedEntity);
        }
    }

    [Test]
    public async Task Get_Should_Return_Null_When_Entity_Not_Found()
    {
        var allData = CreateData();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);

            var result = await _sut.Get(string.Empty);
            result.Should().BeNull();
        }
    }

    [Test]
    public async Task Get_Should_Get_All_Entities()
    {
        var allData = CreateData();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);

            var result = await _sut.Get();
            result.Should().NotBeNull().And.BeEquivalentTo(allData);
        }
    }

    [Test]
    public async Task Get_Should_Return_Empty_List_When_No_Data_Found()
    {
        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            _sut = new ExampleRepository(context);

            var result = await _sut.Get();
            result.Should().BeEmpty();
        }
    }

    [Test]
    public async Task GetByDescription_Should_Get_Single_Entity()
    {
        var allData = CreateData();
        var expectedEntity = allData.First();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);

            var result = await _sut.GetByDescription(expectedEntity.Description);
            result.Should().NotBeNull().And.BeEquivalentTo(expectedEntity);
        }
    }

    [Test]
    public async Task GetByDescription_Should_Return_Null_When_Entity_Not_Found()
    {
        var allData = CreateData();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);

            var result = await _sut.GetByDescription(string.Empty);
            result.Should().BeNull();
        }
    }

    [Test]
    public async Task Create_Should_Save_Entity_To_Database()
    {
        var entity = CreateData().First();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            _sut = new ExampleRepository(context);
            await _sut.Create(entity);

            var dbEntity = context.Examples.FirstOrDefault(e => e.Id == entity.Id);

            dbEntity.Should().NotBeNull().And.BeEquivalentTo(entity);
        }
    }

    [Test]
    public async Task Update_Should_Save_Entity_To_Database()
    {
        var newDescription = _fixture.Create<string>();
        var updatedBy = _fixture.Create<string>();

        var allData = CreateData();

        var entity = allData.First();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);
            entity.Update(newDescription, updatedBy);

            await _sut.Update(entity);

            var dbEntity = context.Examples.FirstOrDefault(e => e.Description == newDescription);

            dbEntity.Should().NotBeNull().And.BeEquivalentTo(entity);
            dbEntity!.Description.Should().Be(newDescription);
            dbEntity.UpdatedBy.Should().Be(updatedBy);
        }
    }

    [Test]
    public async Task Delete_Should_Remove_Entity()
    {
        var allData = CreateData();
        var entity = allData.First();

        await using (var context = new ExampleContext(_contextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Examples.AddRangeAsync(allData);
            await context.SaveChangesAsync();

            _sut = new ExampleRepository(context);

            await _sut.Delete(entity);

            var dbEntity = context.Examples.FirstOrDefault(e => e.Id == entity.Id);

            dbEntity.Should().BeNull();
        }
    }

    private List<ExampleDomainEntity> CreateData()
    {
        var allData = new List<ExampleDomainEntity>();

        for (var i = 0; i < 10; i++)
        {
            var description = _fixture.Create<string>();
            var createdBy = _fixture.Create<string>();

            var entity = new ExampleDomainEntity();
            entity.Create(description, createdBy);
            allData.Add(entity);
        }

        return allData;
    }
}