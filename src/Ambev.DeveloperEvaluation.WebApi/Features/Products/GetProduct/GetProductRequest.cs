namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Request model for retrieving a product
/// </summary>
public sealed class GetProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to retrieve
    /// </summary>
    public Guid Id { get; set; }
} 