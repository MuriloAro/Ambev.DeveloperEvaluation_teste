namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItem;

/// <summary>
/// Response model for cart item addition
/// </summary>
public sealed class AddItemResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    public decimal TotalAmount { get; set; }
} 