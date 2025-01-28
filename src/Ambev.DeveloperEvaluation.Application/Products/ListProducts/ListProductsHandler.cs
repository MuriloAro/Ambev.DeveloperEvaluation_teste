using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler : IRequestHandler<ListProductsQuery, ListProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ListProductsHandler> _logger;

    public ListProductsHandler(
        IProductRepository productRepository,
        ILogger<ListProductsHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ListProductsResult> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing products: {@Request}", new { request.Page, request.PageSize, request.Status, request.Category });
        
        try 
        {
            if (request.Page <= 0)
            {
                _logger.LogWarning("Invalid page number: {Page}", request.Page);
                throw new ValidationException("Page must be greater than zero");
            }

            if (request.PageSize <= 0 || request.PageSize > 100)
            {
                _logger.LogWarning("Invalid page size: {PageSize}", request.PageSize);
                throw new ValidationException("Page size must be between 1 and 100");
            }

            var (items, totalCount) = await _productRepository.ListAsync(
                request.Page,
                request.PageSize,
                request.Status,
                request.Category,
                cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            _logger.LogInformation("Products listed successfully: {@Result}", new { TotalCount = totalCount, TotalPages = totalPages });

            return new ListProductsResult
            {
                Items = items.Select(p => new ProductDto
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing products: {@Request}", new { request.Page, request.PageSize });
            throw;
        }
    }
} 