using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

/// <summary>
/// Command for deactivating a product
/// </summary>
public sealed class DeactivateProductCommand : IRequest<DeactivateProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to deactivate
    /// </summary>
    public Guid Id { get; set; }
}