using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.GetProduct;

public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductHandler(_productRepository);
    }

    [Fact]
    public async Task Given_ValidQuery_When_Handle_Then_ShouldReturnProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductQuery { Id = productId };

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

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(product.Id);
        result.Name.Should().Be(product.Name);
        result.Description.Should().Be(product.Description);
        result.Price.Should().Be(product.Price);
        result.StockQuantity.Should().Be(product.StockQuantity);
        result.Category.Should().Be(product.Category);
        result.Status.Should().Be(product.Status);
        result.CreatedAt.Should().Be(product.CreatedAt);
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetProductQuery { Id = Guid.NewGuid() };
        _productRepository.GetByIdAsync(query.Id).Returns((Product?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(query, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {query.Id} not found");
    }

    [Fact]
    public async Task Given_EmptyId_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var query = new GetProductQuery { Id = Guid.Empty };

        // Act & Assert
        var action = async () => await _handler.Handle(query, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Product Id is required");
    }
} 