using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

/// <summary>
/// Command for completing a sale
/// </summary>
public sealed class CompleteSaleCommand : IRequest<CompleteSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to complete
    /// </summary>
    public Guid Id { get; set; }
}