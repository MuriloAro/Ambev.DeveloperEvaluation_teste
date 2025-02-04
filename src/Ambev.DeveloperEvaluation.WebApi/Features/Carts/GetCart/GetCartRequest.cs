namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// Request model for retrieving a cart
/// </summary>
public sealed class GetCartRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart to retrieve
    /// </summary>
    public Guid Id { get; set; }
} 