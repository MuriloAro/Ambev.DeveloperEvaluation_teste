using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Carts.AddItem;

/// <summary>
/// Handler for processing cart item addition commands
/// </summary>
public sealed class AddItemHandler : IRequestHandler<AddItemCommand, AddItemResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of the AddItemHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="productRepository">The product repository</param>
    public AddItemHandler(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the cart item addition command
    /// </summary>
    /// <param name="request">The addition command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the addition operation</returns>
    public async Task<AddItemResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new DomainException("Product not found");
        }

        cart.AddItem(product, request.Quantity);
        await _cartRepository.UpdateAsync(cart);

        return new AddItemResult
        {
            CartId = cart.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = product.Price,
            Success = true
        };
    }
} 