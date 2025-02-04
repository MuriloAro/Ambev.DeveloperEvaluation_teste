using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

/// <summary>
/// Handler for processing product deactivation commands
/// </summary>
public sealed class DeactivateProductHandler : IRequestHandler<DeactivateProductCommand, DeactivateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeactivateProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the DeactivateProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="logger">The logger instance</param>
    public DeactivateProductHandler(IProductRepository productRepository, ILogger<DeactivateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the product deactivation command
    /// </summary>
    /// <param name="request">The deactivation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the deactivation operation</returns>
    public async Task<DeactivateProductResult> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deactivating product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new DomainException("Product not found");
        }

        product.Deactivate();
        await _productRepository.UpdateAsync(product);

        _logger.LogInformation("Product deactivated successfully");

        return new DeactivateProductResult
        {
            Success = true,
            Message = "Product deactivated successfully"
        };
    }
} 