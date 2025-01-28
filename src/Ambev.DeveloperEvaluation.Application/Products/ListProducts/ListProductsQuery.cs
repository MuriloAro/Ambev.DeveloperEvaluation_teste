using MediatR;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsQuery : IRequest<ListProductsResult>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public ProductStatus? Status { get; set; }
    public ProductCategory? Category { get; set; }
}

public class ListProductsResult
{
    public IEnumerable<ProductDto> Items { get; set; } = new List<ProductDto>();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public ProductCategory Category { get; set; }
    public ProductStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 