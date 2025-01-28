using Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateStock;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Products;

public class ProductsControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new ProductsController(_mediator, _mapper);
    }

    [Fact]
    public async Task Given_ValidRequest_When_Create_Then_ShouldReturnOkWithResult()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.00m,
            StockQuantity = 100,
            Category = ProductCategory.Beer
        };

        var command = new CreateProductCommand
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category
        };

        var expectedResult = new CreateProductResult
        {
            Success = true,
            Message = "Product created successfully",
            ProductId = Guid.NewGuid()
        };

        _mapper.Map<CreateProductCommand>(request).Returns(command);
        _mediator.Send(Arg.Any<CreateProductCommand>()).Returns(expectedResult);

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<CreateProductResult>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Product created successfully");
        response.Data.Should().BeEquivalentTo(expectedResult);

        await _mediator.Received(1).Send(
            Arg.Is<CreateProductCommand>(c => 
                c.Name == request.Name &&
                c.Description == request.Description &&
                c.Price == request.Price &&
                c.StockQuantity == request.StockQuantity &&
                c.Category == request.Category),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_InvalidRequest_When_Create_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "",
            Description = "Test Description",
            Price = 10.00m,
            StockQuantity = 100,
            Category = ProductCategory.Beer
        };

        var errorMessage = "Name is required";
        _mapper.Map<CreateProductCommand>(request).Returns(new CreateProductCommand());
        _mediator.Send(Arg.Any<CreateProductCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_ValidRequest_When_Update_Then_ShouldReturnOkWithResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var request = new UpdateProductRequest
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 20.00m,
            Category = ProductCategory.Beer
        };

        var command = new UpdateProductCommand
        {
            Id = productId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category
        };

        var expectedResult = new UpdateProductResult
        {
            Success = true,
            Message = "Product updated successfully"
        };

        _mapper.Map<UpdateProductCommand>(request).Returns(command);
        _mediator.Send(Arg.Any<UpdateProductCommand>()).Returns(expectedResult);

        // Act
        var result = await _controller.Update(productId, request, CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<UpdateProductResult>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Product updated successfully");
        response.Data.Should().BeEquivalentTo(expectedResult);

        await _mediator.Received(1).Send(
            Arg.Is<UpdateProductCommand>(c => 
                c.Id == productId &&
                c.Name == request.Name &&
                c.Description == request.Description &&
                c.Price == request.Price &&
                c.Category == request.Category),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Update_Then_ShouldReturnNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var request = new UpdateProductRequest();
        var errorMessage = $"Product with ID {productId} not found";

        _mapper.Map<UpdateProductCommand>(request).Returns(new UpdateProductCommand());
        _mediator.Send(Arg.Any<UpdateProductCommand>()).ThrowsAsync(new KeyNotFoundException(errorMessage));

        // Act
        var result = await _controller.Update(productId, request, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_InvalidRequest_When_Update_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new UpdateProductRequest();
        var errorMessage = "Name is required";

        _mapper.Map<UpdateProductCommand>(request).Returns(new UpdateProductCommand());
        _mediator.Send(Arg.Any<UpdateProductCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Update(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_ValidId_When_Get_Then_ShouldReturnOkWithProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var result = new GetProductResult
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

        var expectedResponse = new GetProductResponse
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Price = result.Price,
            StockQuantity = result.StockQuantity,
            Category = result.Category,
            Status = result.Status,
            CreatedAt = result.CreatedAt
        };

        _mediator.Send(Arg.Any<GetProductQuery>()).Returns(result);
        _mapper.Map<GetProductResponse>(result).Returns(expectedResponse);

        // Act
        var actionResult = await _controller.Get(productId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<GetProductResponse>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Product retrieved successfully");
        response.Data.Should().BeEquivalentTo(expectedResponse);

        await _mediator.Received(1).Send(
            Arg.Is<GetProductQuery>(q => q.Id == productId),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Get_Then_ShouldReturnNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var errorMessage = $"Product with ID {productId} not found";
        _mediator.Send(Arg.Any<GetProductQuery>()).ThrowsAsync(new KeyNotFoundException(errorMessage));

        // Act
        var result = await _controller.Get(productId, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_InvalidId_When_Get_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Product Id is required";
        _mediator.Send(Arg.Any<GetProductQuery>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Get(Guid.Empty, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_ValidParameters_When_List_Then_ShouldReturnOkWithProducts()
    {
        // Arrange
        var result = new ListProductsResult
        {
            Items = new List<ProductDto>
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
            },
            TotalCount = 2,
            CurrentPage = 1,
            PageSize = 10,
            TotalPages = 1
        };

        var expectedResponse = new ListProductsResponse
        {
            Items = result.Items.Select(p => new ProductItemResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }),
            TotalCount = result.TotalCount,
            CurrentPage = result.CurrentPage,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        };

        _mediator.Send(Arg.Any<ListProductsQuery>()).Returns(result);
        _mapper.Map<ListProductsResponse>(result).Returns(expectedResponse);

        // Act
        var actionResult = await _controller.List(1, 10, ProductStatus.Active, ProductCategory.Beer, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<ListProductsResponse>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Products retrieved successfully");
        response.Data.Should().BeEquivalentTo(expectedResponse);

        await _mediator.Received(1).Send(
            Arg.Is<ListProductsQuery>(q => 
                q.Page == 1 &&
                q.PageSize == 10 &&
                q.Status == ProductStatus.Active &&
                q.Category == ProductCategory.Beer),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_InvalidParameters_When_List_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Page must be greater than zero";
        _mediator.Send(Arg.Any<ListProductsQuery>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.List(0, 10, null, null, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_NoFilters_When_List_Then_ShouldReturnAllProducts()
    {
        // Arrange
        var result = new ListProductsResult
        {
            Items = new List<ProductDto>
            {
                new() { Id = Guid.NewGuid(), Category = ProductCategory.Beer, Status = ProductStatus.Active },
                new() { Id = Guid.NewGuid(), Category = ProductCategory.Water, Status = ProductStatus.Inactive }
            },
            TotalCount = 2,
            CurrentPage = 1,
            PageSize = 10,
            TotalPages = 1
        };

        var expectedResponse = new ListProductsResponse
        {
            Items = result.Items.Select(p => new ProductItemResponse { Id = p.Id, Category = p.Category, Status = p.Status }),
            TotalCount = result.TotalCount,
            CurrentPage = result.CurrentPage,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        };

        _mediator.Send(Arg.Any<ListProductsQuery>()).Returns(result);
        _mapper.Map<ListProductsResponse>(result).Returns(expectedResponse);

        // Act
        var actionResult = await _controller.List(cancellationToken: CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<ListProductsResponse>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Data?.Items.Select(p => p.Category).Should().Contain(new[] { ProductCategory.Beer, ProductCategory.Water });
        response.Data?.Items.Select(p => p.Status).Should().Contain(new[] { ProductStatus.Active, ProductStatus.Inactive });

        await _mediator.Received(1).Send(
            Arg.Is<ListProductsQuery>(q => 
                q.Page == 1 &&
                q.PageSize == 10 &&
                q.Status == null &&
                q.Category == null),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_ValidId_When_Activate_Then_ShouldReturnOkWithResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedResult = new ActivateProductResult
        {
            Success = true,
            Message = "Product activated successfully"
        };

        _mediator.Send(Arg.Any<ActivateProductCommand>()).Returns(expectedResult);

        // Act
        var actionResult = await _controller.Activate(productId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<ActivateProductResult>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Product activated successfully");
        response.Data.Should().BeEquivalentTo(expectedResult);

        await _mediator.Received(1).Send(
            Arg.Is<ActivateProductCommand>(c => c.Id == productId),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Activate_Then_ShouldReturnNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var errorMessage = $"Product with ID {productId} not found";
        _mediator.Send(Arg.Any<ActivateProductCommand>()).ThrowsAsync(new KeyNotFoundException(errorMessage));

        // Act
        var result = await _controller.Activate(productId, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_AlreadyActiveProduct_When_Activate_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Product is already active";
        _mediator.Send(Arg.Any<ActivateProductCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Activate(Guid.NewGuid(), CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_EmptyId_When_Activate_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Product Id is required";
        _mediator.Send(Arg.Any<ActivateProductCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Activate(Guid.Empty, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_ValidId_When_Deactivate_Then_ShouldReturnOkWithResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedResult = new DeactivateProductResult
        {
            Success = true,
            Message = "Product deactivated successfully"
        };

        _mediator.Send(Arg.Any<DeactivateProductCommand>()).Returns(expectedResult);

        // Act
        var actionResult = await _controller.Deactivate(productId, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<DeactivateProductResult>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Product deactivated successfully");
        response.Data.Should().BeEquivalentTo(expectedResult);

        await _mediator.Received(1).Send(
            Arg.Is<DeactivateProductCommand>(c => c.Id == productId),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_Deactivate_Then_ShouldReturnNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var errorMessage = $"Product with ID {productId} not found";
        _mediator.Send(Arg.Any<DeactivateProductCommand>()).ThrowsAsync(new KeyNotFoundException(errorMessage));

        // Act
        var result = await _controller.Deactivate(productId, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_AlreadyInactiveProduct_When_Deactivate_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Product is already inactive";
        _mediator.Send(Arg.Any<DeactivateProductCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Deactivate(Guid.NewGuid(), CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_EmptyId_When_Deactivate_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Product Id is required";
        _mediator.Send(Arg.Any<DeactivateProductCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.Deactivate(Guid.Empty, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_ValidRequest_When_UpdateStock_Then_ShouldReturnOkWithResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var request = new UpdateStockRequest { StockQuantity = 150 };
        var expectedResult = new UpdateStockResult
        {
            Success = true,
            Message = "Stock updated successfully"
        };

        _mediator.Send(Arg.Any<UpdateStockCommand>()).Returns(expectedResult);

        // Act
        var actionResult = await _controller.UpdateStock(productId, request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ApiResponseWithData<UpdateStockResult>>().Subject;
        
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Stock updated successfully");
        response.Data.Should().BeEquivalentTo(expectedResult);

        await _mediator.Received(1).Send(
            Arg.Is<UpdateStockCommand>(c => 
                c.Id == productId &&
                c.StockQuantity == request.StockQuantity),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_NonExistingProduct_When_UpdateStock_Then_ShouldReturnNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var request = new UpdateStockRequest { StockQuantity = 100 };
        var errorMessage = $"Product with ID {productId} not found";
        _mediator.Send(Arg.Any<UpdateStockCommand>()).ThrowsAsync(new KeyNotFoundException(errorMessage));

        // Act
        var result = await _controller.UpdateStock(productId, request, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_InactiveProduct_When_UpdateStock_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Cannot update stock of inactive product";
        var request = new UpdateStockRequest { StockQuantity = 100 };
        _mediator.Send(Arg.Any<UpdateStockCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.UpdateStock(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Given_NegativeStock_When_UpdateStock_Then_ShouldReturnBadRequest()
    {
        // Arrange
        var errorMessage = "Stock quantity cannot be negative";
        var request = new UpdateStockRequest { StockQuantity = -10 };
        _mediator.Send(Arg.Any<UpdateStockCommand>()).ThrowsAsync(new ValidationException(errorMessage));

        // Act
        var result = await _controller.UpdateStock(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be(errorMessage);
    }
} 