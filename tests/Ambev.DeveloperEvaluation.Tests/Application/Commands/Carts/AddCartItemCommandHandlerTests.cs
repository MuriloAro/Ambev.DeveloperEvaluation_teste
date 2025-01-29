using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Tests.Application.Commands.Carts;

public class AddCartItemCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<AddCartItemCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddCartItemCommandHandler _handler;

    public AddCartItemCommandHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<AddCartItemCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new AddCartItemCommandHandler(
            _cartRepositoryMock.Object,
            _productRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldAddItemToCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new AddCartItemCommand 
        { 
            CartId = cartId,
            ProductId = productId,
            Quantity = 1
        };

        var cart = new Cart(Guid.NewGuid());
        var product = new Product 
        { 
            Id = productId,
            StockQuantity = 10,
            Price = 10.00m
        };
        var cartDto = new CartDto { Id = cartId };

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId))
            .ReturnsAsync(cart);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        _mapperMock.Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cartId);

        _cartRepositoryMock.Verify(x => x.UpdateAsync(cart), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenInsufficientStock_ShouldThrowDomainException()
    {
        // Arrange
        var command = new AddCartItemCommand 
        { 
            CartId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 10
        };

        var cart = new Cart(Guid.NewGuid());
        var product = new Product 
        { 
            Id = command.ProductId,
            StockQuantity = 5
        };

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(command.CartId))
            .ReturnsAsync(cart);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(command.ProductId))
            .ReturnsAsync(product);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Insufficient stock");
    }
} 