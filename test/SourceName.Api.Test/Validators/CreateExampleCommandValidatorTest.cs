using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SourceName.Api.Validators;
using SourceName.Application.Commands;

namespace SourceName.Api.Test.Validators;

[TestFixture]
public class CreateExampleCommandValidatorTest
{
    private Fixture _fixture = null!;
    private CreateExampleCommandValidator _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _sut = new CreateExampleCommandValidator();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Should_Have_Error_When_Description_Is_Null_Or_Empty(string? description)
    {
        var model = new CreateExample.CreateCommand(description!);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Description);
    }

    [Test]
    public void Should_Not_Have_Error_When_Description_Specified()
    {
        var description = _fixture.Create<string>();
        var model = new CreateExample.CreateCommand(description);
        var result = _sut.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}