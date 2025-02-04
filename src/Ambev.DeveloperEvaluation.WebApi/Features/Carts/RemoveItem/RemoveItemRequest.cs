namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.RemoveItem;

/// <summary>
/// Request model for removing an item from a cart
/// </summary>
public sealed class RemoveItemRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart item to remove
    /// </summary>
    public Guid ItemId { get; set; }
} 