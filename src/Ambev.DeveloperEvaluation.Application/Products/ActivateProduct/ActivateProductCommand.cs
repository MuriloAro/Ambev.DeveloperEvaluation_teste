using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

public class ActivateProductCommand : IRequest<ActivateProductResult>
{
    public Guid Id { get; set; }
}

public class ActivateProductResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
} 