namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItem;

/// <summary>
/// Request model for adding an item to a cart
/// </summary>
public sealed class AddItemRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to add
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product to add
    /// Must be greater than 0
    /// </summary>
    public int Quantity { get; set; }
} 