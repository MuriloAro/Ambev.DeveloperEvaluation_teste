using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.AddItem;

/// <summary>
/// Command for adding an item to a cart
/// </summary>
public sealed class AddItemCommand : IRequest<AddItemResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product to add
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity to add
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }
} 