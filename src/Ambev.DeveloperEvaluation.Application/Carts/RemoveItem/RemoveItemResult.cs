namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItem;

/// <summary>
/// Result model for cart item removal operation
/// </summary>
public sealed class RemoveItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the removed product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }
} 