namespace Ambev.DeveloperEvaluation.Application.Carts.Create;

/// <summary>
/// Result model for cart creation operation
/// </summary>
public sealed class CreateResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the created cart
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID who owns the cart
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets when the cart was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 