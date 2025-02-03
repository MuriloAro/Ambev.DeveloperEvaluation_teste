using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class RefreshTokenTests
{
    [Fact]
    public void Given_NonExpiredToken_When_CheckingIsActive_Then_ShouldReturnTrue()
    {
        // Arrange
        var token = new RefreshToken
        {
            Token = "valid_token",
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        token.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Given_ExpiredToken_When_CheckingIsActive_Then_ShouldReturnFalse()
    {
        // Arrange
        var token = new RefreshToken
        {
            Token = "expired_token",
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };

        // Act & Assert
        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Given_RevokedToken_When_CheckingIsActive_Then_ShouldReturnFalse()
    {
        // Arrange
        var token = new RefreshToken
        {
            Token = "revoked_token",
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
            RevokedAt = DateTime.UtcNow
        };

        // Act & Assert
        token.IsActive.Should().BeFalse();
    }
} 