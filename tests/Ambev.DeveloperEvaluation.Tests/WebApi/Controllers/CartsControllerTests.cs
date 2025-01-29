using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Controllers;
using Ambev.DeveloperEvaluation.WebApi.Models;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItem;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.Checkout;
using Ambev.DeveloperEvaluation.Application.DTOs;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Tests.WebApi.Controllers;

public class CartsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<CartsController>> _loggerMock;
    private readonly CartsController _controller;

    public CartsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<CartsController>>();
        _controller = new CartsController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddItem_WhenValidRequest_ShouldReturnOk()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var request = new AddCartItemRequest 
        { 
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        var cartDto = new CartDto { Id = cartId };
        _mediatorMock.Setup(x => x.Send(It.IsAny<AddCartItemCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartDto);

        // Act
        var result = await _controller.AddItem(cartId, request);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(cartDto);
    }

    [Fact]
    public async Task Checkout_WhenValidRequest_ShouldReturnOk()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var request = new CheckoutCartRequest 
        { 
            BranchId = Guid.NewGuid()
        };

        var saleDto = new SaleDto { Id = Guid.NewGuid() };
        _mediatorMock.Setup(x => x.Send(It.IsAny<CheckoutCartCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(saleDto);

        // Act
        var result = await _controller.Checkout(cartId, request);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(saleDto);
    }

    [Fact]
    public async Task Create_WhenValidRequest_ShouldReturnOk()
    {
        // Arrange
        var command = new CreateCartCommand { UserId = Guid.NewGuid() };
        var cartDto = new CartDto { Id = Guid.NewGuid() };

        _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartDto);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(cartDto);
    }

    [Fact]
    public async Task GetById_WhenCartExists_ShouldReturnOk()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var cartDto = new CartDto { Id = cartId };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartDto);

        // Act
        var result = await _controller.GetById(cartId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(cartDto);
    }

    [Fact]
    public async Task GetActiveByUserId_WhenCartExists_ShouldReturnOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cartDto = new CartDto { Id = Guid.NewGuid() };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetActiveCartByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartDto);

        // Act
        var result = await _controller.GetActiveByUserId(userId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(cartDto);
    }

    [Fact]
    public async Task GetActiveByUserId_WhenCartNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetActiveCartByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartDto)null);

        // Act
        var result = await _controller.GetActiveByUserId(userId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task RemoveItem_WhenValidRequest_ShouldReturnOk()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cartDto = new CartDto { Id = cartId };

        _mediatorMock.Setup(x => x.Send(It.IsAny<RemoveCartItemCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cartDto);

        // Act
        var result = await _controller.RemoveItem(cartId, productId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(cartDto);
    }
} 