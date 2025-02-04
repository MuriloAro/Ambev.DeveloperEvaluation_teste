namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Checkout;

/// <summary>
/// Request model for cart checkout
/// </summary>
public sealed class CheckoutRequest
{
    /// <summary>
    /// Gets or sets the branch ID where the purchase will be made
    /// </summary>
    public Guid BranchId { get; set; }
} 