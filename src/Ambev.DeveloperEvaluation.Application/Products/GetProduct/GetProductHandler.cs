using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductHandler> _logger;

    public GetProductHandler(
        IProductRepository productRepository,
        ILogger<GetProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting product: {Id}", request.Id);
        
        try 
        {
            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("Invalid product id: empty guid");
                throw new ValidationException("Product Id is required");
            }

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product not found: {Id}", request.Id);
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");
            }

            _logger.LogInformation("Product retrieved successfully: {@Product}", new { product.Id, product.Name });
            return new GetProductResult
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                Status = product.Status,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product: {Id}", request.Id);
            throw;
        }
    }
} 