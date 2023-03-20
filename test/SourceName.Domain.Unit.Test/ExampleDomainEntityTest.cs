using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SourceName.SharedKernel;

namespace SourceName.Domain.Test;

public class ExampleDomainEntityTest
{
    private Fixture _fixture = null!;
    private ExampleDomainEntity _sut = null!;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _sut = new ExampleDomainEntity();
    }

    [Test]
    public void Create_Should_Assign_Property_Values()
    {
        var description = _fixture.Create<string>();
        var createdBy = _fixture.Create<string>();
        var now = DateTime.Now;

        using (DateTimeProvider.InjectActualDateTime(now))
        {
            _sut.Create(description, createdBy);

            _sut.Id.Should().NotBeEmpty();
            _sut.Description.Should().Be(description);
            _sut.CreatedBy.Should().Be(createdBy);
            _sut.CreatedDate.Should().Be(now);
            _sut.UpdatedDate.Should().Be(now);
            _sut.UpdatedBy.Should().BeNull();
        }
    }

    [Test]
    public void Update_Should_Assign_Property_Values()
    {
        var description1 = _fixture.Create<string>();
        var description2 = _fixture.Create<string>();
        var createdBy = _fixture.Create<string>();
        var updatedBy = _fixture.Create<string>();
        var now = DateTime.Now;
        var yesterday = now.AddDays(-1);

        // create first set of variables
        using (DateTimeProvider.InjectActualDateTime(yesterday))
        {
            _sut.Create(description1, createdBy);
        }

        using (DateTimeProvider.InjectActualDateTime(now))
        {
            _sut.Update(description2, updatedBy);

            // new values
            _sut.Description.Should().Be(description2);
            _sut.UpdatedBy.Should().Be(updatedBy);
            _sut.UpdatedDate.Should().Be(now);

            // make sure old values didn't change
            _sut.CreatedBy.Should().Be(createdBy);
            _sut.CreatedDate.Should().Be(yesterday);
        }
    }
}