using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

/// <summary>
/// Handler for processing product activation commands
/// </summary>
public sealed class ActivateProductHandler : IRequestHandler<ActivateProductCommand, ActivateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ActivateProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the ActivateProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="logger">The logger instance</param>
    public ActivateProductHandler(IProductRepository productRepository, ILogger<ActivateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the product activation command
    /// </summary>
    /// <param name="request">The activation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the activation operation</returns>
    public async Task<ActivateProductResult> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activating product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new DomainException("Product not found");
        }

        product.Activate();
        await _productRepository.UpdateAsync(product);

        _logger.LogInformation("Product activated successfully");

        return new ActivateProductResult
        {
            Id = product.Id,
            Success = true,
            UpdatedAt = product.UpdatedAt
        };
    }
} 