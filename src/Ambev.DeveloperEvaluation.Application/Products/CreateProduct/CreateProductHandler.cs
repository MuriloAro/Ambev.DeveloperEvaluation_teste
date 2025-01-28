using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await ValidateCommand(request);

        var existingProduct = await _productRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingProduct != null)
            throw new ValidationException("A product with this name already exists");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);

        return new CreateProductResult
        {
            Success = true,
            Message = "Product created successfully",
            ProductId = createdProduct.Id
        };
    }

    private static Task ValidateCommand(CreateProductCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException("Name is required");

        if (string.IsNullOrWhiteSpace(command.Description))
            throw new ValidationException("Description is required");

        if (command.Price <= 0)
            throw new ValidationException("Price must be greater than zero");

        if (command.StockQuantity < 0)
            throw new ValidationException("Stock quantity cannot be negative");

        return Task.CompletedTask;
    }
} 