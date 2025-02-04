using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Response model for product listing
/// </summary>
public sealed class ListProductsResponse
{
    /// <summary>
    /// Gets or sets the list of products
    /// </summary>
    public IEnumerable<ProductItemResponse> Items { get; set; } = new List<ProductItemResponse>();

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

/// <summary>
/// Response model for product item in list
/// </summary>
public sealed class ProductItemResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the product category
    /// </summary>
    public ProductCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the product status
    /// </summary>
    public ProductStatus Status { get; set; }

    /// <summary>
    /// Gets or sets when the product was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the product was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 