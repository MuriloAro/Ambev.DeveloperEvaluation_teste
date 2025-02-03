using Ambev.DeveloperEvaluation.Application.Auth.RefreshToken;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var refreshToken = user.RefreshTokens.Single(r => r.Token == request.RefreshToken);
        if (!refreshToken.IsActive)
            throw new UnauthorizedAccessException("Inactive refresh token");

        // Gerar novo access token
        var accessToken = _jwtTokenGenerator.GenerateToken(user);
        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(5);

        // Gerar novo refresh token
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

        // Revogar token antigo
        user.RevokeRefreshToken(refreshToken.Token, "Replaced by new token", newRefreshToken);
        
        // Adicionar novo token
        user.AddRefreshToken(newRefreshToken, refreshTokenExpiresAt);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new RefreshTokenResult
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt
        };
    }
}