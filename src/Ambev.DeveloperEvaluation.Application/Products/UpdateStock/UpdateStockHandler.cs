using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, UpdateStockResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateStockHandler> _logger;

    public UpdateStockHandler(
        IProductRepository productRepository,
        ILogger<UpdateStockHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<UpdateStockResult> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating stock: {@Request}", new { request.Id, request.StockQuantity });
        
        try 
        {
            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("Invalid product id: empty guid");
                throw new ValidationException("Product Id is required");
            }

            if (request.StockQuantity < 0)
            {
                _logger.LogWarning("Invalid stock quantity: {StockQuantity}", request.StockQuantity);
                throw new ValidationException("Stock quantity cannot be negative");
            }

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product not found: {Id}", request.Id);
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");
            }

            if (product.Status == ProductStatus.Inactive)
            {
                _logger.LogWarning("Cannot update stock of inactive product: {Id}", request.Id);
                throw new ValidationException("Cannot update stock of inactive product");
            }

            var oldStock = product.StockQuantity;
            product.StockQuantity = request.StockQuantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);
            _logger.LogInformation("Stock updated successfully: {@StockUpdate}", new 
            { 
                ProductId = product.Id,
                ProductName = product.Name,
                OldStock = oldStock,
                NewStock = product.StockQuantity
            });

            return new UpdateStockResult
            {
                Success = true,
                Message = "Stock updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating stock: {@Request}", new { request.Id, request.StockQuantity });
            throw;
        }
    }
} 