using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.UpdateProduct;

public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new UpdateProductHandler(_productRepository);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldUpdateProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand
        {
            Id = productId,
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 20.00m,
            Category = ProductCategory.Beer
        };

        var existingProduct = new Product
        {
            Id = productId,
            Name = "Original Product",
            Description = "Original Description",
            Price = 10.00m,
            Category = ProductCategory.Beer,
            Status = ProductStatus.Active
        };

        _productRepository.GetByIdAsync(productId).Returns(existingProduct);
        _productRepository.GetByNameAsync(command.Name).Returns((Product?)null);
        _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(existingProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Product updated successfully");

        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<Product>(p =>
                p.Id == command.Id &&
                p.Name == command.Name &&
                p.Description == command.Description &&
                p.Price == command.Price &&
                p.Category == command.Category &&
                p.UpdatedAt != null
            )
        );
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateProductCommand 
        { 
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.00m,
            Category = ProductCategory.Beer
        };
        _productRepository.GetByIdAsync(command.Id).Returns((Product?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found");
    }

    [Fact]
    public async Task Given_DuplicateName_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand
        {
            Id = productId,
            Name = "Existing Product",
            Description = "Description",
            Price = 10.00m,
            Category = ProductCategory.Beer
        };

        var existingProduct = new Product { Id = productId };
        var duplicateProduct = new Product { Id = Guid.NewGuid() };

        _productRepository.GetByIdAsync(productId).Returns(existingProduct);
        _productRepository.GetByNameAsync(command.Name).Returns(duplicateProduct);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("A product with this name already exists");
    }

    [Theory]
    [InlineData("", "Description", 10.00)]
    [InlineData("Name", "", 10.00)]
    [InlineData("Name", "Description", 0)]
    [InlineData("Name", "Description", -1)]
    public async Task Given_InvalidCommand_When_Handle_Then_ShouldThrowValidationException(
        string name,
        string description,
        decimal price)
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            Category = ProductCategory.Beer
        };

        // Configurar o mock para retornar um produto
        var existingProduct = new Product { Id = command.Id };
        _productRepository.GetByIdAsync(command.Id).Returns(existingProduct);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>();
    }
} 