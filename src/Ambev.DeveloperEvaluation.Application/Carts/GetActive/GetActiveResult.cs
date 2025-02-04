using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetActive;

/// <summary>
/// Result model for retrieving active cart
/// </summary>
public sealed class GetActiveResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID who owns the cart
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the cart
    /// </summary>
    public ICollection<CartItemResult> Items { get; set; } = new List<CartItemResult>();

    /// <summary>
    /// Gets or sets the cart status
    /// </summary>
    public CartStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the total amount of the cart
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets when the cart was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the cart was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Result model for cart item
/// </summary>
public sealed class CartItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the item
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the item
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }
} 