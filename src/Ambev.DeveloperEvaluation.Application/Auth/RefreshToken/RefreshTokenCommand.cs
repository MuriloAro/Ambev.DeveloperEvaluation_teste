using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResult>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}