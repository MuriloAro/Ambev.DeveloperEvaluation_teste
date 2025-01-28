using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper);
    }

    [Fact]
    public async Task Given_ValidCommand_When_Handle_Then_ShouldCreateSale()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 5, UnitPrice = 10.00m }
            }
        };

        var expectedResult = new CreateSaleResult
        {
            Id = Guid.NewGuid(),
            Number = "SALE-123",
            TotalAmount = 45.00m
        };

        _saleRepository.CreateAsync(Arg.Any<Sale>()).Returns(new Sale(command.CustomerId, command.BranchId));
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>());
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task Given_InvalidCommand_When_Handle_Then_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand(); // Invalid command without required fields

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
} 