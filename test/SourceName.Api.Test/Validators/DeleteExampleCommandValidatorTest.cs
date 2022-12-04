using FluentValidation.TestHelper;
using NUnit.Framework;
using SourceName.Api.Validators;
using SourceName.Application.Commands;

namespace SourceName.Api.Test.Validators;

[TestFixture]
public class DeleteExampleCommandValidatorTest
{
    private DeleteExampleCommandValidator _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new DeleteExampleCommandValidator();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Should_Have_Error_When_Id_Is_Null_Or_Empty(string? id)
    {
        var model = new DeleteExample.Command(id!);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void Should_Have_Error_When_Id_Is_Too_Short()
    {
        var id = Guid.NewGuid().ToString()[..35];
        var model = new DeleteExample.Command(id!);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void Should_Not_Have_Error_When_Id_Specified()
    {
        var id = Guid.NewGuid().ToString();
        var model = new DeleteExample.Command(id);
        var result = _sut.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}