using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItem;

public class RemoveCartItemCommand : IRequest<CartDto>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
}

public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("CartId is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required");
    }
}

public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<RemoveCartItemCommandHandler> _logger;
    private readonly IMapper _mapper;

    public RemoveCartItemCommandHandler(
        ICartRepository cartRepository,
        ILogger<RemoveCartItemCommandHandler> logger,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing item from cart {CartId}", request.CartId);

        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        cart.RemoveItem(request.ProductId);
        await _cartRepository.UpdateAsync(cart);

        _logger.LogInformation("Item removed from cart {CartId} successfully", cart.Id);

        return _mapper.Map<CartDto>(cart);
    }
} 