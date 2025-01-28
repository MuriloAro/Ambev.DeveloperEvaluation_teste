using Ambev.DeveloperEvaluation.Application.Products.UpdateStock;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.UpdateStock;

public class UpdateStockHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateStockHandler> _logger;
    private readonly UpdateStockHandler _handler;

    public UpdateStockHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<UpdateStockHandler>>();
        _handler = new UpdateStockHandler(_productRepository, _logger);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldUpdateStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateStockCommand 
        { 
            Id = productId,
            StockQuantity = 150
        };

        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.00m,
            StockQuantity = 100,
            Category = ProductCategory.Beer,
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _productRepository.GetByIdAsync(productId).Returns(product);
        _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Stock updated successfully");

        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<Product>(p =>
                p.Id == productId &&
                p.StockQuantity == 150 &&
                p.UpdatedAt != null
            )
        );
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateStockCommand 
        { 
            Id = Guid.NewGuid(),
            StockQuantity = 100
        };
        _productRepository.GetByIdAsync(command.Id).Returns((Product?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
    }

    [Fact]
    public async Task Given_InactiveProduct_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateStockCommand 
        { 
            Id = productId,
            StockQuantity = 100
        };

        var product = new Product
        {
            Id = productId,
            Status = ProductStatus.Inactive
        };

        _productRepository.GetByIdAsync(productId).Returns(product);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Cannot update stock of inactive product");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task Given_NegativeStock_When_Handle_Then_ShouldThrowValidationException(int stockQuantity)
    {
        // Arrange
        var command = new UpdateStockCommand 
        { 
            Id = Guid.NewGuid(),
            StockQuantity = stockQuantity
        };

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Stock quantity cannot be negative");
    }

    [Fact]
    public async Task Given_EmptyId_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateStockCommand 
        { 
            Id = Guid.Empty,
            StockQuantity = 100
        };

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Product Id is required");
    }
} 