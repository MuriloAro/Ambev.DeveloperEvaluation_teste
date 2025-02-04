using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetActive;

/// <summary>
/// Command for retrieving the active cart for a user
/// </summary>
public sealed class GetActiveCommand : IRequest<GetActiveResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the user
    /// </summary>
    public Guid UserId { get; set; }
} 