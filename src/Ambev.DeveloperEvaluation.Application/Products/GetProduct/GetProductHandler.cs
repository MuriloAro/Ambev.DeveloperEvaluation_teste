using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResult>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
            throw new ValidationException("Product Id is required");

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");

        return new GetProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Category = product.Category,
            Status = product.Status,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
} 