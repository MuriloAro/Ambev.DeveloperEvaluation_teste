using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Application.Auth.RefreshToken;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshTokenFeature;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Auth;

public class AuthControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new AuthController(_mediator, _mapper);
    }

    [Fact]
    public async Task Given_ValidRefreshToken_When_RefreshToken_Then_ShouldReturnNewTokens()
    {
        // Arrange
        var request = new RefreshTokenRequest { RefreshToken = "valid_token" };
        var result = new RefreshTokenResult 
        { 
            AccessToken = "new_access_token",
            RefreshToken = "new_refresh_token",
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
        var response = new RefreshTokenResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            AccessTokenExpiresAt = result.AccessTokenExpiresAt
        };

        _mediator.Send(Arg.Any<RefreshTokenCommand>()).Returns(result);
        _mapper.Map<RefreshTokenResponse>(result).Returns(response);

        // Act
        var actionResult = await _controller.RefreshToken(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var apiResponse = okResult.Value.Should().BeOfType<ApiResponseWithData<RefreshTokenResponse>>().Subject;
        
        apiResponse.Success.Should().BeTrue();
        apiResponse.Message.Should().Be("Token refreshed successfully");
        apiResponse.Data.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Given_InvalidRefreshToken_When_RefreshToken_Then_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new RefreshTokenRequest { RefreshToken = "invalid_token" };
        _mediator.Send(Arg.Any<RefreshTokenCommand>())
            .ThrowsAsync(new UnauthorizedAccessException("Invalid refresh token"));

        // Act
        var actionResult = await _controller.RefreshToken(request, CancellationToken.None);

        // Assert
        var unauthorizedResult = actionResult.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        var apiResponse = unauthorizedResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        apiResponse.Success.Should().BeFalse();
        apiResponse.Message.Should().Be("Invalid refresh token");
    }

    [Fact]
    public async Task Given_ValidCredentials_When_AuthenticateUser_Then_ShouldReturnTokens()
    {
        // Arrange
        var request = new AuthenticateUserRequest 
        { 
            Email = "admin@ambev.com",
            Password = "Admin123!"
        };

        var commandResult = new AuthenticateUserResult
        {
            AccessToken = "test_access_token",
            RefreshToken = "test_refresh_token",
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(5),
            Email = request.Email,
            Name = "Admin",
            Role = "Admin"
        };

        var response = new AuthenticateUserResponse
        {
            Token = commandResult.AccessToken,
            RefreshToken = commandResult.RefreshToken,
            AccessTokenExpiresAt = commandResult.AccessTokenExpiresAt,
            Email = commandResult.Email,
            Name = commandResult.Name,
            Role = commandResult.Role
        };

        _mediator.Send(Arg.Any<AuthenticateUserCommand>()).Returns(commandResult);
        _mapper.Map<AuthenticateUserResponse>(commandResult).Returns(response);

        // Act
        var result = await _controller.AuthenticateUser(request, CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var apiResponse = okResult.Value.Should().BeOfType<ApiResponseWithData<AuthenticateUserResponse>>().Subject;
        
        apiResponse.Success.Should().BeTrue();
        apiResponse.Message.Should().Be("User authenticated successfully");
        apiResponse.Data.Should().BeEquivalentTo(response);
        apiResponse.Data!.RefreshToken.Should().NotBeNullOrEmpty();
        apiResponse.Data.AccessTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(5), TimeSpan.FromSeconds(1));
    }
} 