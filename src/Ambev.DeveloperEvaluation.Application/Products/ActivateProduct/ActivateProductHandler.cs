using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

public class ActivateProductHandler : IRequestHandler<ActivateProductCommand, ActivateProductResult>
{
    private readonly IProductRepository _productRepository;

    public ActivateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ActivateProductResult> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ValidationException("Product Id is required");

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        if (product.Status == ProductStatus.Active)
            throw new ValidationException("Product is already active");

        product.Status = ProductStatus.Active;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new ActivateProductResult
        {
            Success = true,
            Message = "Product activated successfully"
        };
    }
} 