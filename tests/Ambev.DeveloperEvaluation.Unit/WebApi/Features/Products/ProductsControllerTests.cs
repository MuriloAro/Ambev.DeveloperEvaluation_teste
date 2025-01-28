using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
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
} 