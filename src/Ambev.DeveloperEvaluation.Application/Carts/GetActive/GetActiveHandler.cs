using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetActive;

/// <summary>
/// Handler for processing get active cart commands
/// </summary>
public sealed class GetActiveHandler : IRequestHandler<GetActiveCommand, GetActiveResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetActiveHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The automapper instance</param>
    public GetActiveHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get active cart command
    /// </summary>
    /// <param name="request">The get active cart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The active cart result</returns>
    public async Task<GetActiveResult> Handle(GetActiveCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);
        if (cart == null)
        {
            throw new DomainException("No active cart found for user");
        }

        return _mapper.Map<GetActiveResult>(cart);
    }
} 