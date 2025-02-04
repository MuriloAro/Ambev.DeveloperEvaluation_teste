namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CompleteSale;

/// <summary>
/// Request model for completing a sale
/// </summary>
public sealed class CompleteSaleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to complete
    /// </summary>
    public Guid Id { get; set; }
} 