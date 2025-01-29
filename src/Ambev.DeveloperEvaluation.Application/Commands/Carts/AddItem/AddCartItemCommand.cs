using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItem;

public class AddCartItemCommand : IRequest<CartDto>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemCommandValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("CartId is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");
    }
}

public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<AddCartItemCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddCartItemCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ILogger<AddCartItemCommandHandler> logger,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding item to cart {CartId}", request.CartId);

        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new DomainException("Product not found");
        }

        if (product.StockQuantity < request.Quantity)
        {
            throw new DomainException("Insufficient stock");
        }

        cart.AddItem(product, request.Quantity);
        await _cartRepository.UpdateAsync(cart);

        _logger.LogInformation("Item added to cart {CartId} successfully", cart.Id);

        return _mapper.Map<CartDto>(cart);
    }
} 