using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Request model for updating a product
/// </summary>
public sealed class UpdateProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name
    /// Must be between 3 and 100 characters
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// Must be greater than 0
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product category
    /// </summary>
    public ProductCategory Category { get; set; }
} 