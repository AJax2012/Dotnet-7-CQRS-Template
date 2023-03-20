using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Common.Dtos;
using SourceName.Application.Common.Errors;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Application.Test.Commands;

[TestFixture]
public class CreateExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private Mock<ICurrentUserService> _currentUserServiceMock = null!;
    private Mock<IValidationHandler<CreateExample.CreateCommand>> _validatorMock = null!;
    private CreateExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _validatorMock = new Mock<IValidationHandler<CreateExample.CreateCommand>>();
        _sut = new CreateExample.Handler(_repositoryMock.Object, _currentUserServiceMock.Object, _validatorMock.Object);
    }

    [Test]
    public async Task Should_Call_Validator_Validate()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.CreateCommand(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _validatorMock.Setup(v => v.Validate(
            It.Is<CreateExample.CreateCommand>(c =>
                c.Description == description)))
            .ReturnsAsync(ValidationResult.Success)
            .Verifiable();

        _currentUserServiceMock.Setup(c => c.Username)
            .Returns(username);

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity);

        await _sut.Handle(command, CancellationToken.None);

        _validatorMock.Verify();
    }

    [Test]
    public async Task Should_Return_EntityExistsError_When_Validation_Fails()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.CreateCommand(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _validatorMock.Setup(v => v.Validate(
                It.Is<CreateExample.CreateCommand>(c =>
                    c.Description == description)))
            .ReturnsAsync(ValidationResult.Fail("Description already exists"));

        var actual = await _sut.Handle(command, CancellationToken.None);

        actual.IsError.Should().Be(true);
        actual.Errors.Should().NotBeNullOrEmpty().And.HaveCount(1);
        actual.FirstError.Should().Be(Errors.Entity.Exists);
    }

    [Test]
    public async Task Should_Call_CurrentUserService_Username()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.CreateCommand(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _validatorMock.Setup(v => v.Validate(
                It.Is<CreateExample.CreateCommand>(c =>
                    c.Description == description)))
            .ReturnsAsync(ValidationResult.Success);

        _currentUserServiceMock.Setup(c => c.Username)
            .Returns(username)
            .Verifiable();

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity);

        await _sut.Handle(command, CancellationToken.None);

        _currentUserServiceMock.Verify();
    }

    [Test]
    public async Task Should_Call_Repository_Create()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.CreateCommand(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _validatorMock.Setup(v => v.Validate(
                It.Is<CreateExample.CreateCommand>(c =>
                    c.Description == description)))
            .ReturnsAsync(ValidationResult.Success);

        _currentUserServiceMock.Setup(c => c.Username).Returns(username);

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity)
            .Verifiable();

        await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify();
    }

    [Test]
    public async Task Should_Return_Response()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var command = new CreateExample.CreateCommand(description);
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);

        _validatorMock.Setup(v => v.Validate(
                It.Is<CreateExample.CreateCommand>(c =>
                    c.Description == description)))
            .ReturnsAsync(ValidationResult.Success);

        _currentUserServiceMock.Setup(c => c.Username).Returns(username);

        _repositoryMock.Setup(r => r.Create(
                It.Is<ExampleDomainEntity>(e =>
                    e.Description == entity.Description &&
                    e.CreatedBy == entity.CreatedBy)))
            .ReturnsAsync(entity);

        var actual = await _sut.Handle(command, CancellationToken.None);

        actual.Value.Should().NotBeNull().And.BeOfType<CreateExample.CreatedResponse>();
        actual.Value.Id.Should().NotBeEmpty();
        actual.Value.Description.Should().Be(description);
    }
}