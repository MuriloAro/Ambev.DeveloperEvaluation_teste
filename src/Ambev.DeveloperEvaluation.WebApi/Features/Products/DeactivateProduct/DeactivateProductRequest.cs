namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeactivateProduct;

/// <summary>
/// Request model for deactivating a product
/// </summary>
public sealed class DeactivateProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to deactivate
    /// </summary>
    public Guid Id { get; set; }
} 