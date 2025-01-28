using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(
        IProductRepository productRepository,
        ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product: {@Request}", new { request.Id, request.Name });
        
        try 
        {
            if (request.Id == Guid.Empty)
                throw new ValidationException("Product Id is required");

            await ValidateCommand(request);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product not found: {Id}", request.Id);
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");
            }

            var existingProduct = await _productRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingProduct != null && existingProduct.Id != request.Id)
            {
                _logger.LogWarning("Product with name {Name} already exists", request.Name);
                throw new ValidationException("A product with this name already exists");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Category = request.Category;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product, cancellationToken);
            _logger.LogInformation("Product updated successfully: {@Product}", new { product.Id, product.Name });

            return new UpdateProductResult
            {
                Success = true,
                Message = "Product updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product: {Id}", request.Id);
            throw;
        }
    }

    private static Task ValidateCommand(UpdateProductCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException("Name is required");

        if (string.IsNullOrWhiteSpace(command.Description))
            throw new ValidationException("Description is required");

        if (command.Price <= 0)
            throw new ValidationException("Price must be greater than zero");

        return Task.CompletedTask;
    }
} 