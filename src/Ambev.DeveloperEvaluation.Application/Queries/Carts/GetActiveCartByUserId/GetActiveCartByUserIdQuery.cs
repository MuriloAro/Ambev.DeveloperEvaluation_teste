using System;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Queries.Carts.GetActiveCartByUserId;

public class GetActiveCartByUserIdQuery : IRequest<CartDto>
{
    public Guid UserId { get; set; }
}

public class GetActiveCartByUserIdQueryHandler : IRequestHandler<GetActiveCartByUserIdQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetActiveCartByUserIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetActiveCartByUserIdQueryHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ILogger<GetActiveCartByUserIdQueryHandler> logger,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(GetActiveCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting active cart for user {UserId}", request.UserId);

        var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);
        if (cart == null)
        {
            return null; // Usuário não tem carrinho ativo
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