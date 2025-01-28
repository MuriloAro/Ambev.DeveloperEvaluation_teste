using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<ListProductsQuery, ListProductsResult>
{
    private readonly IProductRepository _productRepository;

    public ListProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ListProductsResult> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        await ValidateQuery(request);

        var (products, totalCount) = await _productRepository.ListAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.Category,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new ListProductsResult
        {
            Items = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }),
            TotalCount = totalCount,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }

    private static Task ValidateQuery(ListProductsQuery query)
    {
        if (query.Page <= 0)
            throw new ValidationException("Page must be greater than zero");

        if (query.PageSize <= 0)
            throw new ValidationException("PageSize must be greater than zero");

        if (query.PageSize > 100)
            throw new ValidationException("PageSize must be less than or equal to 100");

        return Task.CompletedTask;
    }
} 