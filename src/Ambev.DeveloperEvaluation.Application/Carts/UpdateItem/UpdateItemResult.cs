namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateItem;

/// <summary>
/// Result model for cart item update operation
/// </summary>
public sealed class UpdateItemResult
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
    /// Gets or sets the updated quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }
} 