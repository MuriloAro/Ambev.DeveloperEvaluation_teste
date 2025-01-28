using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

public class DeactivateProductCommand : IRequest<DeactivateProductResult>
{
    public Guid Id { get; set; }
}

public class DeactivateProductResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
} 