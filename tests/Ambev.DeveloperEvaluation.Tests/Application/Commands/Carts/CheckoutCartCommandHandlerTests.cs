using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.Checkout;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Tests.Application.Commands.Carts;

public class CheckoutCartCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IBranchRepository> _branchRepositoryMock;
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<ILogger<CheckoutCartCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CheckoutCartCommandHandler _handler;

    public CheckoutCartCommandHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _branchRepositoryMock = new Mock<IBranchRepository>();
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _loggerMock = new Mock<ILogger<CheckoutCartCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CheckoutCartCommandHandler(
            _cartRepositoryMock.Object,
            _productRepositoryMock.Object,
            _branchRepositoryMock.Object,
            _saleRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateSaleAndUpdateStock()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CheckoutCartCommand 
        { 
            CartId = cartId,
            BranchId = branchId
        };

        var cart = new Cart(userId);
        var product = new Product 
        { 
            Id = productId,
            StockQuantity = 10,
            Price = 10.00m
        };
        cart.AddItem(product, 2);

        var branch = new Branch("Test Branch", "SP");

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId))
            .ReturnsAsync(cart);

        _branchRepositoryMock.Setup(x => x.GetByIdAsync(branchId))
            .ReturnsAsync(branch);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        var sale = new Sale(userId, branchId);
        sale.AddItem(productId, 2, 10.00m);
        sale.Confirm();
        sale.Complete();
        var saleDto = new SaleDto { Id = sale.Id };

        _saleRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Sale>()))
            .ReturnsAsync(sale);

        _mapperMock.Setup(x => x.Map<SaleDto>(sale))
            .Returns(saleDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);

        _productRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Product>(p => p.StockQuantity == 8)), Times.Once);
        _saleRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Sale>()), Times.Once);
        _cartRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Cart>(c => c.Status == CartStatus.Completed)), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenInsufficientStock_ShouldThrowDomainException()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CheckoutCartCommand 
        { 
            CartId = cartId,
            BranchId = branchId
        };

        var cart = new Cart(userId);
        var product = new Product 
        { 
            Id = productId,
            StockQuantity = 1,
            Price = 10.00m
        };
        cart.AddItem(product, 2);

        var branch = new Branch("Test Branch", "SP");

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId))
            .ReturnsAsync(cart);

        _branchRepositoryMock.Setup(x => x.GetByIdAsync(branchId))
            .ReturnsAsync(branch);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<DomainException>()
            .WithMessage($"Insufficient stock for product {productId}");
    }
} 