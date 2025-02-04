using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Request model for creating a new product
/// </summary>
public sealed class CreateProductRequest
{
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
    /// Gets or sets the initial stock quantity
    /// Must be greater than or equal to 0
    /// </summary>
    public int StockQuantity { get; set; }

    public ProductCategory Category { get; set; }
} 