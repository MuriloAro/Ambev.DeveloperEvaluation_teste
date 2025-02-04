using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItem;

/// <summary>
/// Command for removing an item from a cart
/// </summary>
public sealed class RemoveItemCommand : IRequest<RemoveItemResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product to remove
    /// </summary>
    public Guid ProductId { get; set; }
} 