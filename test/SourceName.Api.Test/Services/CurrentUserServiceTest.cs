using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SourceName.Api.Services;

namespace SourceName.Api.Test.Services;

[TestFixture]
public class CurrentUserServiceTest
{
    private Fixture _fixture = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;
    private CurrentUserService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void Username_Should_Return_Value_When_Name_Valid()
    {
        var username = _fixture.Create<string>();

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContextAccessorMock.Setup(h => h.HttpContext!.User.Identity!.Name).Returns(username);

        _sut = new CurrentUserService(_httpContextAccessorMock.Object);

        var actual = _sut.Username;

        actual.Should().NotBeNull().And.Be(username);
    }
}