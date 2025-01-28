using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.ListProducts;

public class ListProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ListProductsHandler _handler;

    public ListProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new ListProductsHandler(_productRepository);
    }

    [Fact]
    public async Task Given_ValidQuery_When_Handle_Then_ShouldReturnPagedProducts()
    {
        // Arrange
        var query = new ListProductsQuery
        {
            Page = 1,
            PageSize = 10,
            Status = ProductStatus.Active,
            Category = ProductCategory.Beer
        };

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.00m,
                StockQuantity = 100,
                Category = ProductCategory.Beer,
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Description = "Description 2",
                Price = 20.00m,
                StockQuantity = 200,
                Category = ProductCategory.Beer,
                Status = ProductStatus.Active,
                CreatedAt = DateTime.UtcNow
            }
        };

        _productRepository.ListAsync(
            query.Page,
            query.PageSize,
            query.Status,
            query.Category,
            Arg.Any<CancellationToken>()
        ).Returns((products, 2));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.CurrentPage.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalPages.Should().Be(1);

        result.Items.Should().BeEquivalentTo(products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            Category = p.Category,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        }));
    }

    [Theory]
    [InlineData(0, 10)]  // Invalid page
    [InlineData(1, 0)]   // Invalid pageSize
    [InlineData(1, 101)] // PageSize too large
    public async Task Given_InvalidPagination_When_Handle_Then_ShouldThrowValidationException(int page, int pageSize)
    {
        // Arrange
        var query = new ListProductsQuery { Page = page, PageSize = pageSize };

        // Act & Assert
        var action = async () => await _handler.Handle(query, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Given_NoFilters_When_Handle_Then_ShouldReturnAllProducts()
    {
        // Arrange
        var query = new ListProductsQuery
        {
            Page = 1,
            PageSize = 10
        };

        var products = new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Category = ProductCategory.Beer,
                Status = ProductStatus.Active
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Category = ProductCategory.Water,
                Status = ProductStatus.Inactive
            }
        };

        _productRepository.ListAsync(
            query.Page,
            query.PageSize,
            null,
            null,
            Arg.Any<CancellationToken>()
        ).Returns((products, 2));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Select(p => p.Category).Should().Contain(new[] { ProductCategory.Beer, ProductCategory.Water });
        result.Items.Select(p => p.Status).Should().Contain(new[] { ProductStatus.Active, ProductStatus.Inactive });
    }
} 