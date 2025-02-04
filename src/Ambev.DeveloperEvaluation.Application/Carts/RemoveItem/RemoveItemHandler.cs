using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItem;

/// <summary>
/// Handler for processing cart item removal commands
/// </summary>
public sealed class RemoveItemHandler : IRequestHandler<RemoveItemCommand, RemoveItemResult>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes a new instance of the RemoveItemHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    public RemoveItemHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the cart item removal command
    /// </summary>
    /// <param name="request">The removal command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the removal operation</returns>
    public async Task<RemoveItemResult> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        cart.RemoveItem(request.ProductId);
        await _cartRepository.UpdateAsync(cart);

        return new RemoveItemResult
        {
            CartId = cart.Id,
            ProductId = request.ProductId,
            Success = true
        };
    }
} 