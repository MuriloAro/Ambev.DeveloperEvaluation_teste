using Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.ActivateProduct;

public class ActivateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ActivateProductHandler> _logger;
    private readonly ActivateProductHandler _handler;

    public ActivateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<ActivateProductHandler>>();
        _handler = new ActivateProductHandler(_productRepository, _logger);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldActivateProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new ActivateProductCommand { Id = productId };

        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.00m,
            StockQuantity = 100,
            Category = ProductCategory.Beer,
            Status = ProductStatus.Inactive,
            CreatedAt = DateTime.UtcNow
        };

        _productRepository.GetByIdAsync(productId).Returns(product);
        _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Product activated successfully");

        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<Product>(p =>
                p.Id == productId &&
                p.Status == ProductStatus.Active &&
                p.UpdatedAt != null
            )
        );
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new ActivateProductCommand { Id = Guid.NewGuid() };
        _productRepository.GetByIdAsync(command.Id).Returns((Product?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
    }

    [Fact]
    public async Task Given_AlreadyActiveProduct_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new ActivateProductCommand { Id = productId };

        var product = new Product
        {
            Id = productId,
            Status = ProductStatus.Active
        };

        _productRepository.GetByIdAsync(productId).Returns(product);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Product is already active");
    }

    [Fact]
    public async Task Given_EmptyId_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = new ActivateProductCommand { Id = Guid.Empty };

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Product Id is required");
    }
} 