using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Tests.Application.Commands.Carts;

public class RemoveCartItemCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<ILogger<RemoveCartItemCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RemoveCartItemCommandHandler _handler;

    public RemoveCartItemCommandHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _loggerMock = new Mock<ILogger<RemoveCartItemCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new RemoveCartItemCommandHandler(
            _cartRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldRemoveItemFromCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new RemoveCartItemCommand 
        { 
            CartId = cartId,
            ProductId = productId
        };

        var cart = new Cart(Guid.NewGuid());
        var product = new Product { Id = productId, Price = 10.00m };
        cart.AddItem(product, 1);
        
        var cartDto = new CartDto { Id = cartId };

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId))
            .ReturnsAsync(cart);

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
    public async Task Handle_WhenCartNotFound_ShouldThrowDomainException()
    {
        // Arrange
        var command = new RemoveCartItemCommand 
        { 
            CartId = Guid.NewGuid(),
            ProductId = Guid.NewGuid()
        };

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(command.CartId))
            .ReturnsAsync((Cart)null);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Cart not found");
    }
} 