using AutoFixture;

using FluentValidation.TestHelper;

using NUnit.Framework;

using SourceName.Api.Validators;
using SourceName.Application.Commands;

namespace SourceName.Api.Unit.Test.Validators;

[TestFixture]
public class UpdateExampleCommandValidatorTest
{
    private Fixture _fixture = null!;
    private UpdateExampleCommandValidator _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _sut = new UpdateExampleCommandValidator();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Should_Have_Error_When_Id_Is_Null_Or_Empty(string? id)
    {
        var description = _fixture.Create<string>();
        var model = new UpdateExample.UpdateCommand(id!, description);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void Should_Have_Error_When_Id_Is_Too_Short()
    {
        var id = Guid.NewGuid().ToString()[..35];
        var description = _fixture.Create<string>();
        var model = new UpdateExample.UpdateCommand(id, description);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void Should_Not_Have_Error_When_Id_Specified()
    {
        var id = Guid.NewGuid().ToString();
        var description = _fixture.Create<string>();
        var model = new UpdateExample.UpdateCommand(id, description);
        var result = _sut.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Should_Have_Error_When_Description_Is_Null_Or_Empty(string? description)
    {
        var id = Guid.NewGuid().ToString();
        var model = new UpdateExample.UpdateCommand(id, description!);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Description);
    }

    [Test]
    public void Should_Not_Have_Error_When_Description_Specified()
    {
        var id = Guid.NewGuid().ToString();
        var description = _fixture.Create<string>();
        var model = new UpdateExample.UpdateCommand(id, description);
        var result = _sut.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}