using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateItem;

/// <summary>
/// Handler for processing cart item update commands
/// </summary>
public sealed class UpdateItemHandler : IRequestHandler<UpdateItemCommand, UpdateItemResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the UpdateItemHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The automapper instance</param>
    public UpdateItemHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the cart item update command
    /// </summary>
    /// <param name="request">The update command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the update operation</returns>
    public async Task<UpdateItemResult> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
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

        var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (item == null)
        {
            throw new DomainException("Item not found in cart");
        }

        item.UpdateQuantity(request.Quantity);
        await _cartRepository.UpdateAsync(cart);

        return new UpdateItemResult
        {
            CartId = cart.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = item.UnitPrice,
            TotalAmount = item.TotalAmount,
            Success = true
        };
    }
} 