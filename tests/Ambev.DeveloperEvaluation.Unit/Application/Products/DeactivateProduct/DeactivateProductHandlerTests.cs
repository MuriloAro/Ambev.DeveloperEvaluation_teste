using Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.DeactivateProduct;

public class DeactivateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeactivateProductHandler> _logger;
    private readonly DeactivateProductHandler _handler;

    public DeactivateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<DeactivateProductHandler>>();
        _handler = new DeactivateProductHandler(_productRepository, _logger);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldDeactivateProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeactivateProductCommand { Id = productId };

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
        result.Message.Should().Be("Product deactivated successfully");

        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<Product>(p =>
                p.Id == productId &&
                p.Status == ProductStatus.Inactive &&
                p.UpdatedAt != null
            )
        );
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeactivateProductCommand { Id = Guid.NewGuid() };
        _productRepository.GetByIdAsync(command.Id).Returns((Product?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
    }

    [Fact]
    public async Task Given_AlreadyInactiveProduct_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeactivateProductCommand { Id = productId };

        var product = new Product
        {
            Id = productId,
            Status = ProductStatus.Inactive
        };

        _productRepository.GetByIdAsync(productId).Returns(product);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Product is already inactive");
    }

    [Fact]
    public async Task Given_EmptyId_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = new DeactivateProductCommand { Id = Guid.Empty };

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Product Id is required");
    }
} 