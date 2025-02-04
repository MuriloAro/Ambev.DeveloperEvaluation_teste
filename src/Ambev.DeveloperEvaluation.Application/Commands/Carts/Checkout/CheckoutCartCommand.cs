using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.Checkout;

public class CheckoutCartCommand : IRequest<SaleDto>
{
    public Guid CartId { get; set; }
    public Guid BranchId { get; set; }
}

public class CheckoutCartCommandValidator : AbstractValidator<CheckoutCartCommand>
{
    public CheckoutCartCommandValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("CartId is required");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("BranchId is required");
    }
}

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, SaleDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CheckoutCartCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CheckoutCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IBranchRepository branchRepository,
        ISaleRepository saleRepository,
        ILogger<CheckoutCartCommandHandler> logger,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _saleRepository = saleRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<SaleDto> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing checkout for cart {CartId}", request.CartId);

        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new DomainException("Cart not found");
        }

        var branch = await _branchRepository.GetByIdAsync(request.BranchId);
        if (branch == null)
        {
            throw new DomainException("Branch not found");
        }

        // Verificar estoque de todos os produtos
        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null || product.StockQuantity < item.Quantity)
            {
                throw new DomainException($"Insufficient stock for product {item.ProductId}");
            }
        }

        // Criar venda
        var sale = new Sale(
            customerId: cart.UserId,
            branchId: request.BranchId
        );

        // Adicionar itens e atualizar estoque
        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            
            sale.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
            product.UpdateStock(product.StockQuantity - item.Quantity);
            
            await _productRepository.UpdateAsync(product);
        }

        //// Confirmar e completar a venda
        //sale.Confirm();
        //sale.Complete();

        // Salvar venda
        sale = await _saleRepository.CreateAsync(sale);

        // Completar carrinho
        cart.Complete();
        await _cartRepository.UpdateAsync(cart);

        _logger.LogInformation("Checkout completed successfully for cart {CartId}. Sale {SaleId} created", cart.Id, sale.Id);

        return _mapper.Map<SaleDto>(sale);
    }
} 