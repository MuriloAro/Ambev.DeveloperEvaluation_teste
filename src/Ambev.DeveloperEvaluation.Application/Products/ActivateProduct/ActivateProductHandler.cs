using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

public class ActivateProductHandler : IRequestHandler<ActivateProductCommand, ActivateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ActivateProductHandler> _logger;

    public ActivateProductHandler(
        IProductRepository productRepository,
        ILogger<ActivateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ActivateProductResult> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activating product: {Id}", request.Id);
        
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

            if (product.Status == ProductStatus.Active)
            {
                _logger.LogWarning("Product already active: {Id}", request.Id);
                throw new ValidationException("Product is already active");
            }

            product.Status = ProductStatus.Active;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);
            _logger.LogInformation("Product activated successfully: {@Product}", new { product.Id, product.Name });

            return new ActivateProductResult
            {
                Success = true,
                Message = "Product activated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating product: {Id}", request.Id);
            throw;
        }
    }
} 