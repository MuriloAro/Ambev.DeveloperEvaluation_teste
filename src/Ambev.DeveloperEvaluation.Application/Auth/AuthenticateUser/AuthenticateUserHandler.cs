using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticateUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var activeUserSpec = new ActiveUserSpecification();
            if (!activeUserSpec.IsSatisfiedBy(user))
            {
                throw new UnauthorizedAccessException("User is not active");
            }

            // Gerar access token
            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(5);

            // Gerar refresh token
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            // Adicionar refresh token ao usuário
            user.AddRefreshToken(refreshToken, refreshTokenExpiresAt);
            await _userRepository.UpdateAsync(user, cancellationToken);

            return new AuthenticateUserResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                Email = user.Email,
                Name = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}
