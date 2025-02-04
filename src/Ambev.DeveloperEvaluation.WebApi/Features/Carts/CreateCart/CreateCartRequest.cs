namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Request model for creating a new cart
/// </summary>
public sealed class CreateCartRequest
{
    /// <summary>
    /// Gets or sets the customer ID who owns the cart
    /// </summary>
    public Guid CustomerId { get; set; }
} 