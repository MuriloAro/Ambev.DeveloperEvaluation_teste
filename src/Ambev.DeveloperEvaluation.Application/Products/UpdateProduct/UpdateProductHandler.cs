using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ValidationException("Product Id is required");

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        await ValidateCommand(request);

        var existingProduct = await _productRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingProduct != null && existingProduct.Id != request.Id)
            throw new ValidationException("A product with this name already exists");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Category = request.Category;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateProductResult
        {
            Success = true,
            Message = "Product updated successfully"
        };
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