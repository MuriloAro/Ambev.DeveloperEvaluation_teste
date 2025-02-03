using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.Application.Auth.RefreshToken;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshTokenFeature;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of AuthController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Authenticates a user with their credentials
    /// </summary>
    /// <param name="request">The authentication request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication token if successful</returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<AuthenticateUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var validator = new AuthenticateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<AuthenticateUserCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<AuthenticateUserResponse>
        {
            Success = true,
            Message = "User authenticated successfully",
            Data = _mapper.Map<AuthenticateUserResponse>(response)
        });
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new RefreshTokenCommand { RefreshToken = request.RefreshToken };
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<RefreshTokenResponse>
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = _mapper.Map<RefreshTokenResponse>(result)
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
