using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Application.Test.Commands;

[TestFixture]
public class CreateExampleValidatorTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private CreateExample.Validator _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _sut = new CreateExample.Validator(_repositoryMock.Object);
    }

    [Test]
    public async Task Should_Call_Repository_GetByDescription()
    {
        var description = _fixture.Create<string>();
        var command = new CreateExample.Command(description);

        await _sut.Validate(command);

        _repositoryMock.Verify(
            r => r.GetByDescription(It.Is<string>(s => s == description)),
            Times.Once);
    }

    [Test]
    public async Task Should_Return_Failure_When_Result_Not_Null()
    {
        var description = _fixture.Create<string>();
        var command = new CreateExample.Command(description);
        var expectedResult = ValidationResult.Fail("Description already exists");

        _repositoryMock.Setup(r =>
                r.GetByDescription(It.Is<string>(s => s == description)))
            .ReturnsAsync(new ExampleDomainEntity());

        var actual = await _sut.Validate(command);

        actual.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task Should_Return_Success_When_Result_Null()
    {
        var description = _fixture.Create<string>();
        var command = new CreateExample.Command(description);
        var expectedResult = ValidationResult.Success;

        var actual = await _sut.Validate(command);

        actual.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
    }
}