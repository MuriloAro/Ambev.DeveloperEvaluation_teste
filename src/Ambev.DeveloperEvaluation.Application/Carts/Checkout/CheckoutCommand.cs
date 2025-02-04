using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Checkout;

/// <summary>
/// Command for checking out a cart
/// </summary>
public sealed class CheckoutCommand : IRequest<CheckoutResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart to checkout
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the branch where the sale will be processed
    /// </summary>
    public Guid BranchId { get; set; }
} 