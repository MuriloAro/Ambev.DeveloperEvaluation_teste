using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Sales;

public class SalesControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new SalesController(_mediator, _mapper);
    }

    [Fact]
    public async Task Given_ExistingSaleId_When_GetById_Then_ShouldReturnOkWithSale()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var getSaleResult = new GetSaleResult
        {
            Id = saleId,
            Number = "SALE-123",
            Date = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Status = SaleStatus.Pending,
            TotalAmount = 100.00m
        };

        var expectedResponse = new GetSaleResponse
        {
            Id = saleId,
            Number = "SALE-123",
            Status = SaleStatus.Pending,
            TotalAmount = 100.00m
        };

        _mediator.Send(Arg.Any<GetSaleQuery>()).Returns(getSaleResult);
        _mapper.Map<GetSaleResponse>(getSaleResult).Returns(expectedResponse);

        // Act
        var result = await _controller.GetById(saleId, CancellationToken.None);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var apiResponse = okResult.Value.Should().BeOfType<ApiResponseWithData<GetSaleResponse>>().Subject;
        
        apiResponse.Success.Should().BeTrue();
        apiResponse.Message.Should().Be("Sale retrieved successfully");
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data.Should().BeEquivalentTo(expectedResponse);

        await _mediator.Received(1).Send(Arg.Is<GetSaleQuery>(q => q.Id == saleId));
    }

    [Fact]
    public async Task Given_NonExistingSaleId_When_GetById_Then_ShouldReturnNotFound()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        _mediator.Send(Arg.Any<GetSaleQuery>()).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.GetById(saleId, CancellationToken.None);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        
        response.Success.Should().BeFalse();
        response.Message.Should().Be($"Sale with ID {saleId} not found");
    }
} 