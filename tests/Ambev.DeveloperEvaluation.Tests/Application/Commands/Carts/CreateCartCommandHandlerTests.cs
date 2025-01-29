using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Tests.Application.Commands.Carts;

public class CreateCartCommandHandlerTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<CreateCartCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateCartCommandHandler _handler;

    public CreateCartCommandHandlerTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<CreateCartCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateCartCommandHandler(
            _cartRepositoryMock.Object,
            _userRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldCreateCart()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateCartCommand { UserId = userId };
        var user = new User { Id = userId };
        var cart = new Cart(userId);
        var cartDto = new CartDto { Id = cart.Id };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _cartRepositoryMock.Setup(x => x.GetActiveCartByUserIdAsync(userId))
            .ReturnsAsync((Cart)null);

        _cartRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Cart>()))
            .ReturnsAsync(cart);

        _mapperMock.Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);

        _cartRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Once);
    }
} 