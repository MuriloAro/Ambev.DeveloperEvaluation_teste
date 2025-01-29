using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Tests.Application.Queries.Carts;

public class GetCartByIdQueryHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<GetCartByIdQueryHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCartByIdQueryHandler _handler;

    public GetCartByIdQueryHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetCartByIdQueryHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetCartByIdQueryHandler(
            _cartRepositoryMock.Object,
            _productRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCartExists_ShouldReturnCartDto()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var query = new GetCartByIdQuery { Id = cartId };
        var cart = new Cart(userId);
        var product = new Product 
        { 
            Id = productId,
            Name = "Test Product",
            Price = 10.00m
        };
        cart.AddItem(product, 1);

        var cartDto = new CartDto { Id = cartId };
        cartDto.Items = new List<CartItemDto> 
        { 
            new CartItemDto { ProductId = productId }
        };

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId))
            .ReturnsAsync(cart);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        _mapperMock.Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cartId);
        result.Items.Should().ContainSingle();
        result.Items.First().ProductName.Should().Be("Test Product");
    }

    [Fact]
    public async Task Handle_WhenCartNotFound_ShouldThrowDomainException()
    {
        // Arrange
        var query = new GetCartByIdQuery { Id = Guid.NewGuid() };

        _cartRepositoryMock.Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync((Cart)null);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Cart not found");
    }
} 