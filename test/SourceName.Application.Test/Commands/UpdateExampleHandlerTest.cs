using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SourceName.Application.Commands;
using SourceName.Application.Contracts;
using SourceName.Domain;

namespace SourceName.Application.Test.Commands;

[TestFixture]
public class UpdateExampleHandlerTest
{
    private Fixture _fixture = null!;
    private Mock<IRepository> _repositoryMock = null!;
    private Mock<ICurrentUserService> _currentUserServiceMock = null!;
    private UpdateExample.Handler _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _repositoryMock = new Mock<IRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _sut = new UpdateExample.Handler(_repositoryMock.Object, _currentUserServiceMock.Object);
    }

    [Test]
    public async Task Should_Call_CurrentUserService_Username()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new UpdateExample.Command(entity.Id, description);
        
        _currentUserServiceMock.Setup(c => c.Username)
            .Returns(username)
            .Verifiable();
        
        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(e => e == entity.Id)))
            .ReturnsAsync(entity);
        
        _repositoryMock.Setup(r => r.Update(
                It.Is<ExampleDomainEntity>(e => 
                    e.Id == entity.Id &&
                    e.Description == entity.Description &&
                    e.UpdatedBy == entity.UpdatedBy &&
                    e.CreatedDate == entity.CreatedDate &&
                    e.UpdatedDate == entity.UpdatedDate)))
            .ReturnsAsync(entity);

        await _sut.Handle(command, CancellationToken.None);
        
        _currentUserServiceMock.Verify();
    }

    [Test]
    public async Task Should_Call_Repository_Get()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new UpdateExample.Command(entity.Id, description);
        
        _currentUserServiceMock.Setup(c => c.Username).Returns(username);
        
        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(e => e == entity.Id)))
            .ReturnsAsync(entity)
            .Verifiable();
        
        _repositoryMock.Setup(r => r.Update(
                It.Is<ExampleDomainEntity>(e => 
                    e.Id == entity.Id &&
                    e.Description == entity.Description &&
                    e.UpdatedBy == entity.UpdatedBy &&
                    e.CreatedDate == entity.CreatedDate &&
                    e.UpdatedDate == entity.UpdatedDate)))
            .ReturnsAsync(entity);
    
        await _sut.Handle(command, CancellationToken.None);
        
        _repositoryMock.Verify();
    }
    

    [Test]
    public async Task Should_Call_Repository_Update()
    {
        var description = _fixture.Create<string>();
        var username = _fixture.Create<string>();
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new UpdateExample.Command(entity.Id, description);
        
        _currentUserServiceMock.Setup(c => c.Username).Returns(username);
        
        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(e => e == entity.Id)))
            .ReturnsAsync(entity);
        
        _repositoryMock.Setup(r => r.Update(
                It.Is<ExampleDomainEntity>(e => 
                    e.Id == entity.Id &&
                    e.Description == entity.Description &&
                    e.UpdatedBy == entity.UpdatedBy &&
                    e.CreatedDate == entity.CreatedDate &&
                    e.UpdatedDate == entity.UpdatedDate)))
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
        var entity = new ExampleDomainEntity();
        entity.Create(description, username);
        var command = new UpdateExample.Command(entity.Id, description);
        
        _currentUserServiceMock.Setup(c => c.Username).Returns(username);
        
        _repositoryMock.Setup(r => r.Get(
                It.Is<string>(e => e == entity.Id)))
            .ReturnsAsync(entity);
        
        _repositoryMock.Setup(r => r.Update(
                It.Is<ExampleDomainEntity>(e => 
                    e.Id == entity.Id &&
                    e.Description == entity.Description &&
                    e.UpdatedBy == entity.UpdatedBy &&
                    e.CreatedDate == entity.CreatedDate &&
                    e.UpdatedDate == entity.UpdatedDate)))
            .ReturnsAsync(entity)
            .Verifiable();
    
        var actual = await _sut.Handle(command, CancellationToken.None);
    
        actual.Should().NotBeNull().And.BeOfType<UpdateExample.Response>();
        actual.Id.Should().Be(entity.Id);
        actual.Description.Should().Be(description);
        actual.CreatedDate.Should().Be(entity.CreatedDate);
        actual.UpdatedDate.Should().Be(entity.UpdatedDate);
    }
}