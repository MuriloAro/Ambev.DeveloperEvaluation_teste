namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Result model for sale cancellation operation
/// </summary>
public sealed class CancelSaleResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message describing the result
    /// </summary>
    public string Message { get; set; } = string.Empty;
} 