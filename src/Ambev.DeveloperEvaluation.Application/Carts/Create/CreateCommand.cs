using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Create;

/// <summary>
/// Command for creating a new cart
/// </summary>
public sealed class CreateCommand : IRequest<CreateResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the user who owns the cart
    /// </summary>
    public Guid UserId { get; set; }
} 