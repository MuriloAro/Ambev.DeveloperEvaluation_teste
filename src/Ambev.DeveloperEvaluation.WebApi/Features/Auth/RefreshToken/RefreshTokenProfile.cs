using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Auth.RefreshToken;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshToken;

/// <summary>
/// AutoMapper profile for refresh token mappings
/// </summary>
public sealed class RefreshTokenProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for refresh token operations
    /// </summary>
    public RefreshTokenProfile()
    {
        CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
        CreateMap<RefreshTokenResult, RefreshTokenResponse>();
    }
} 