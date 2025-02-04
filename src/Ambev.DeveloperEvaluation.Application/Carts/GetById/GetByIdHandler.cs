using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetById;

/// <summary>
/// Handler for processing get cart by ID commands
/// </summary>
public sealed class GetByIdHandler : IRequestHandler<GetByIdCommand, GetByIdResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetByIdHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="mapper">The automapper instance</param>
    public GetByIdHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the get cart by ID command
    /// </summary>
    /// <param name="request">The get cart by ID command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart result</returns>
    public async Task<GetByIdResult> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        return _mapper.Map<GetByIdResult>(cart);
    }
} 