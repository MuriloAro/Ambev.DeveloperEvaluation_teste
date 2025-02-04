namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ActivateProduct;

/// <summary>
/// Request model for activating a product
/// </summary>
public sealed class ActivateProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to activate
    /// </summary>
    public Guid Id { get; set; }
} 