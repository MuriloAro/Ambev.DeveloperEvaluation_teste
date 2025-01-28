using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.CreateProduct;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new CreateProductHandler(_productRepository);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldCreateProduct()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.00m,
            StockQuantity = 100,
            Category = ProductCategory.Beer
        };

        var createdProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            StockQuantity = command.StockQuantity,
            Category = command.Category,
            Status = ProductStatus.Active
        };

        _productRepository.CreateAsync(Arg.Any<Product>()).Returns(createdProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Product created successfully");
        result.ProductId.Should().Be(createdProduct.Id);

        await _productRepository.Received(1).CreateAsync(
            Arg.Is<Product>(p =>
                p.Name == command.Name &&
                p.Description == command.Description &&
                p.Price == command.Price &&
                p.StockQuantity == command.StockQuantity &&
                p.Category == command.Category &&
                p.Status == ProductStatus.Active
            )
        );
    }

    [Theory]
    [InlineData("", "Description", 10.00, 100, ProductCategory.Beer)]
    [InlineData("Name", "", 10.00, 100, ProductCategory.Beer)]
    [InlineData("Name", "Description", 0, 100, ProductCategory.Beer)]
    [InlineData("Name", "Description", -1, 100, ProductCategory.Beer)]
    [InlineData("Name", "Description", 10.00, -1, ProductCategory.Beer)]
    public async Task Given_InvalidCommand_When_Handle_Then_ShouldThrowValidationException(
        string name,
        string description,
        decimal price,
        int stockQuantity,
        ProductCategory category)
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            Category = category
        };

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Given_DuplicateName_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "Existing Product",
            Description = "Test Description",
            Price = 10.00m,
            StockQuantity = 100,
            Category = ProductCategory.Beer
        };

        _productRepository.GetByNameAsync(command.Name).Returns(new Product());

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("A product with this name already exists");
    }
} 