using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Request model for listing products
/// </summary>
public sealed class ListProductsRequest
{
    /// <summary>
    /// Gets or sets the page number for pagination
    /// Must be greater than 0
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size for pagination
    /// Must be between 1 and 100
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets whether to include only active products
    /// </summary>
    public bool? OnlyActive { get; set; }

    /// <summary>
    /// Gets or sets the product category to filter
    /// </summary>
    public ProductCategory? Category { get; set; }
} 