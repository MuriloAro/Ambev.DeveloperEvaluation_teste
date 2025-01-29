using System;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;

public class GetCartByIdQuery : IRequest<CartDto>
{
    public Guid Id { get; set; }
}

public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetCartByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetCartByIdQueryHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ILogger<GetCartByIdQueryHandler> logger,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting cart by id {CartId}", request.Id);

        var cart = await _cartRepository.GetByIdAsync(request.Id);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        var cartDto = _mapper.Map<CartDto>(cart);

        // Carregar nomes dos produtos
        foreach (var item in cartDto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            item.ProductName = product?.Name ?? "Unknown Product";
        }

        return cartDto;
    }
} 