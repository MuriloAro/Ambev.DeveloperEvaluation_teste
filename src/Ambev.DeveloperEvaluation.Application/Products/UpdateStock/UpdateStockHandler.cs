using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Handler for processing stock update commands
/// </summary>
public sealed class UpdateStockHandler : IRequestHandler<UpdateStockCommand, UpdateStockResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateStockHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the UpdateStockHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="logger">The logger instance</param>
    public UpdateStockHandler(IProductRepository productRepository, ILogger<UpdateStockHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the stock update command
    /// </summary>
    /// <param name="request">The update command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the update operation</returns>
    public async Task<UpdateStockResult> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating stock for product ID: {ProductId}", request.ProductId);

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new DomainException("Product not found");
        }

        product.UpdateStock(request.Quantity);
        await _productRepository.UpdateAsync(product);

        _logger.LogInformation("Stock updated successfully");

        return new UpdateStockResult
        {
            Success = true,
            Message = "Stock updated successfully"
        };
    }
} 