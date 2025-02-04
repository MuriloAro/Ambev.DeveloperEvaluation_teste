using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateItem;

/// <summary>
/// Command for updating a cart item quantity
/// </summary>
public sealed class UpdateItemCommand : IRequest<UpdateItemResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the new quantity for the item
    /// </summary>
    public int Quantity { get; set; }
} 