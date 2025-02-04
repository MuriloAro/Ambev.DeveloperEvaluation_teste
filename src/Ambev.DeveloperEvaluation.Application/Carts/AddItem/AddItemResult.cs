namespace Ambev.DeveloperEvaluation.Application.Carts.AddItem;

/// <summary>
/// Result model for cart item addition operation
/// </summary>
public sealed class AddItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the added product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity added
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }
} 