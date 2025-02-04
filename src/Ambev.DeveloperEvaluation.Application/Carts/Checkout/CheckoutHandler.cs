using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Carts.Checkout;

/// <summary>
/// Handler for processing cart checkout commands
/// </summary>
public sealed class CheckoutHandler : IRequestHandler<CheckoutCommand, CheckoutResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CheckoutHandler> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CheckoutHandler
    /// </summary>
    public CheckoutHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IBranchRepository branchRepository,
        ISaleRepository saleRepository,
        ILogger<CheckoutHandler> logger,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _saleRepository = saleRepository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the cart checkout command
    /// </summary>
    /// <param name="request">The checkout command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the checkout operation</returns>
    public async Task<CheckoutResult> Handle(CheckoutCommand request, CancellationToken cancellationToken)
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
        var sale = new Sale(cart.UserId, request.BranchId);

        // Adicionar itens e atualizar estoque
        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            sale.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
            product.UpdateStock(product.StockQuantity - item.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        // Salvar venda
        sale = await _saleRepository.CreateAsync(sale);

        // Completar carrinho
        cart.Complete();
        await _cartRepository.UpdateAsync(cart);

        _logger.LogInformation("Checkout completed successfully for cart {CartId}. Sale {SaleId} created", cart.Id, sale.Id);

        return _mapper.Map<CheckoutResult>(sale);
    }
} 