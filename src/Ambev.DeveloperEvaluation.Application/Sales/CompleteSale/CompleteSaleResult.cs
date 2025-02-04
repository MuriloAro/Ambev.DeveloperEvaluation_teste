namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

/// <summary>
/// Result model for sale completion operation
/// </summary>
public sealed class CompleteSaleResult
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