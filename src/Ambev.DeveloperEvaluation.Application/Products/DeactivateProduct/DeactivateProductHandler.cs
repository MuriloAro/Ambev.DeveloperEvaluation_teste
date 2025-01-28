using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

public class DeactivateProductHandler : IRequestHandler<DeactivateProductCommand, DeactivateProductResult>
{
    private readonly IProductRepository _productRepository;

    public DeactivateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<DeactivateProductResult> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ValidationException("Product Id is required");

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        if (product.Status == ProductStatus.Inactive)
            throw new ValidationException("Product is already inactive");

        product.Status = ProductStatus.Inactive;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new DeactivateProductResult
        {
            Success = true,
            Message = "Product deactivated successfully"
        };
    }
} 