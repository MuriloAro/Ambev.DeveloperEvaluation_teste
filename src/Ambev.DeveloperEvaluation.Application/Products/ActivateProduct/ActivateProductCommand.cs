using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

/// <summary>
/// Command for activating a product
/// </summary>
public sealed class ActivateProductCommand : IRequest<ActivateProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to activate
    /// </summary>
    public Guid Id { get; set; }
}