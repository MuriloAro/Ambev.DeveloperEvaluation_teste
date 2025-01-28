using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Fact]
    public async Task Given_ExistingSaleId_When_Handle_Then_ShouldReturnSale()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleQuery(saleId);
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var expectedResult = new GetSaleResult 
        { 
            Id = saleId,
            Number = "SALE-123",
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            TotalAmount = 100.00m
        };

        _saleRepository.GetByIdAsync(saleId).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _saleRepository.Received(1).GetByIdAsync(saleId);
    }

    [Fact]
    public async Task Given_NonExistingSaleId_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleQuery(saleId);

        _saleRepository.GetByIdAsync(saleId).Returns((Sale?)null);

        // Act & Assert
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {saleId} not found");
    }
} 