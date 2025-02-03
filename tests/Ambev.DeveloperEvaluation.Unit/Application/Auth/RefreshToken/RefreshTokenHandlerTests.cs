using Ambev.DeveloperEvaluation.Application.Auth.RefreshToken;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth.RefreshToken;

public class RefreshTokenHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly RefreshTokenHandler _handler;

    public RefreshTokenHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new RefreshTokenHandler(_userRepository, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Given_ValidRefreshToken_When_Handle_Then_ShouldReturnNewTokens()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "valid_refresh_token" };
        var user = new User { Id = Guid.NewGuid() };
        var newAccessToken = "new_access_token";
        var newRefreshToken = "new_refresh_token";

        user.AddRefreshToken("valid_refresh_token", DateTime.UtcNow.AddDays(7));

        _userRepository.GetByRefreshTokenAsync(command.RefreshToken).Returns(user);
        _jwtTokenGenerator.GenerateToken(user).Returns(newAccessToken);
        _jwtTokenGenerator.GenerateRefreshToken().Returns(newRefreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be(newAccessToken);
        result.RefreshToken.Should().Be(newRefreshToken);
        result.AccessTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(5), TimeSpan.FromSeconds(1));

        await _userRepository.Received(1).UpdateAsync(
            Arg.Is<User>(u => 
                u.RefreshTokens.Any(rt => rt.Token == newRefreshToken) &&
                u.RefreshTokens.Any(rt => 
                    rt.Token == "valid_refresh_token" && 
                    rt.RevokedAt != null &&
                    rt.ReplacedByToken == newRefreshToken)
            )
        );
    }

    [Fact]
    public async Task Given_InvalidRefreshToken_When_Handle_Then_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "invalid_token" };
        _userRepository.GetByRefreshTokenAsync(command.RefreshToken).Returns((User?)null);

        // Act & Assert
        var action = () => _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid refresh token");
    }

    [Fact]
    public async Task Given_ExpiredRefreshToken_When_Handle_Then_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "expired_token" };
        var user = new User { Id = Guid.NewGuid() };
        
        user.AddRefreshToken("expired_token", DateTime.UtcNow.AddDays(-1));

        _userRepository.GetByRefreshTokenAsync(command.RefreshToken).Returns(user);

        // Act & Assert
        var action = () => _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Inactive refresh token");
    }
} 