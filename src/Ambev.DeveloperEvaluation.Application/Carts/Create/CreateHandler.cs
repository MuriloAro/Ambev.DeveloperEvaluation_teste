using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.Create;

/// <summary>
/// Handler for processing cart creation commands
/// </summary>
public sealed class CreateHandler : IRequestHandler<CreateCommand, CreateResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CreateHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The automapper instance</param>
    public CreateHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the cart creation command
    /// </summary>
    /// <param name="request">The creation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the creation operation</returns>
    public async Task<CreateResult> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var cart = new Cart(request.UserId);
        await _cartRepository.AddAsync(cart);

        return _mapper.Map<CreateResult>(cart);
    }
} 