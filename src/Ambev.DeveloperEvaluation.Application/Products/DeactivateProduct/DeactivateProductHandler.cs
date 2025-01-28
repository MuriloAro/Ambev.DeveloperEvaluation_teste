using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

public class DeactivateProductHandler : IRequestHandler<DeactivateProductCommand, DeactivateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeactivateProductHandler> _logger;

    public DeactivateProductHandler(
        IProductRepository productRepository,
        ILogger<DeactivateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<DeactivateProductResult> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deactivating product: {Id}", request.Id);
        
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

            if (product.Status == ProductStatus.Inactive)
            {
                _logger.LogWarning("Product already inactive: {Id}", request.Id);
                throw new ValidationException("Product is already inactive");
            }

            product.Status = ProductStatus.Inactive;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);
            _logger.LogInformation("Product deactivated successfully: {@Product}", new { product.Id, product.Name });

            return new DeactivateProductResult
            {
                Success = true,
                Message = "Product deactivated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating product: {Id}", request.Id);
            throw;
        }
    }
} 