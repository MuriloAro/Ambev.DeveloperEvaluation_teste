using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetActiveCartByUserId;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Tests.Application.Queries.Carts;

public class GetActiveCartByUserIdQueryHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ILogger<GetActiveCartByUserIdQueryHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetActiveCartByUserIdQueryHandler _handler;

    public GetActiveCartByUserIdQueryHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<GetActiveCartByUserIdQueryHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetActiveCartByUserIdQueryHandler(
            _cartRepositoryMock.Object,
            _productRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenActiveCartExists_ShouldReturnCartDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var query = new GetActiveCartByUserIdQuery { UserId = userId };
        var cart = new Cart(userId);
        var product = new Product 
        { 
            Id = productId,
            Name = "Test Product",
            Price = 10.00m
        };
        cart.AddItem(product, 1);

        var cartDto = new CartDto { Id = cart.Id };
        cartDto.Items = new List<CartItemDto> 
        { 
            new CartItemDto { ProductId = productId }
        };

        _cartRepositoryMock.Setup(x => x.GetActiveCartByUserIdAsync(userId))
            .ReturnsAsync(cart);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        _mapperMock.Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        result.Items.Should().ContainSingle();
        result.Items.First().ProductName.Should().Be("Test Product");
    }

    [Fact]
    public async Task Handle_WhenNoActiveCart_ShouldReturnNull()
    {
        // Arrange
        var query = new GetActiveCartByUserIdQuery { UserId = Guid.NewGuid() };

        _cartRepositoryMock.Setup(x => x.GetActiveCartByUserIdAsync(query.UserId))
            .ReturnsAsync((Cart)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
} 