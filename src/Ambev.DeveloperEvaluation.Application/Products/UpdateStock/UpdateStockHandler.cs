using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

public class UpdateStockHandler : IRequestHandler<UpdateStockCommand, UpdateStockResult>
{
    private readonly IProductRepository _productRepository;

    public UpdateStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateStockResult> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ValidationException("Product Id is required");

        if (request.StockQuantity < 0)
            throw new ValidationException("Stock quantity cannot be negative");

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        if (product.Status == ProductStatus.Inactive)
            throw new ValidationException("Cannot update stock of inactive product");

        product.StockQuantity = request.StockQuantity;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdateStockResult
        {
            Success = true,
            Message = "Stock updated successfully"
        };
    }
} 