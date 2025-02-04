using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Result model for product listing operation
/// </summary>
public sealed class ListProductsResult
{
    /// <summary>
    /// Gets or sets the list of products
    /// </summary>
    public ICollection<ProductItemResult> Items { get; set; } = new List<ProductItemResult>();

    /// <summary>
    /// Gets or sets the total number of products
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}