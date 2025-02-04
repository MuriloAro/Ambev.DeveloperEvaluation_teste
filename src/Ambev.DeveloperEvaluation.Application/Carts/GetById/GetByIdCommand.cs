using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetById;

/// <summary>
/// Command for retrieving a cart by its ID
/// </summary>
public sealed class GetByIdCommand : IRequest<GetByIdResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart
    /// </summary>
    public Guid Id { get; set; }
} 