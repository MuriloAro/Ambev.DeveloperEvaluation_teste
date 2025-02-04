using MediatR;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Command for listing products with pagination and filters
/// </summary>
public sealed class ListProductsCommand : IRequest<ListProductsResult>
{
    /// <summary>
    /// Gets or sets the page number (starts at 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (max 100)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the product category filter
    /// </summary>
    public ProductCategory? Category { get; set; }

    /// <summary>
    /// Gets or sets whether to include only active products
    /// </summary>
    public ProductStatus? Status { get; set; }
} 