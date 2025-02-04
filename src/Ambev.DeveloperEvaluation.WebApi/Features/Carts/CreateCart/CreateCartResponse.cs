namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Response model for cart creation
/// </summary>
public sealed class CreateCartResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the created cart
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the customer ID who owns the cart
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets when the cart was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 