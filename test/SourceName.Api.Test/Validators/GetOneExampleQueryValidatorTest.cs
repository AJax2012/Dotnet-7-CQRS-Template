using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SourceName.Api.Validators;
using SourceName.Application.Queries;

namespace SourceName.Api.Test.Validators;

[TestFixture]
public class GetOneExampleQueryValidatorTest
{
    private Fixture _fixture = null!;
    private GetOneExampleQueryValidator _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _sut = new GetOneExampleQueryValidator();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Should_Have_Error_When_Id_Is_Null_Or_Empty(string? id)
    {
        var model = new GetOneExample.Query(id!);
        var result = _sut.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Test]
    public void Should_Not_Have_Error_When_Id_Specified()
    {
        var id = _fixture.Create<string>();
        var model = new GetOneExample.Query(id);
        var result = _sut.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}