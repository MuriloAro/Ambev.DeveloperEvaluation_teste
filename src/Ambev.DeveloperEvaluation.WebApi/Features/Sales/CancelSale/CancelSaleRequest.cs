namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Request model for cancelling a sale
/// </summary>
public sealed class CancelSaleRequest
{
    /// <summary>
    /// Gets or sets the reason for cancellation
    /// </summary>
    public string Reason { get; set; } = string.Empty;
} 