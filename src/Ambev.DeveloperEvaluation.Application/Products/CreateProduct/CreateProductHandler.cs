using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Handler for processing product creation commands
/// </summary>
public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the CreateProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="logger">The logger instance</param>
    public CreateProductHandler(IProductRepository productRepository, ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the product creation command
    /// </summary>
    /// <param name="request">The creation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the creation operation</returns>
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new product: {Name}", request.Name);

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category
        };

        await _productRepository.CreateAsync(product);

        _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

        return new CreateProductResult
        {
            Success = true,
            Message = "Product created successfully",
            ProductId = product.Id
        };
    }
} 