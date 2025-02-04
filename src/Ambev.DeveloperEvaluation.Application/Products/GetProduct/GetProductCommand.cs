using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Command for retrieving a product by ID
/// </summary>
public sealed class GetProductCommand : IRequest<GetProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to retrieve
    /// </summary>
    public Guid Id { get; set; }
} 