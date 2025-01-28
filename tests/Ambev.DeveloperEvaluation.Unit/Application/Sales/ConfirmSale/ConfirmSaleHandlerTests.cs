using Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.ConfirmSale;

public class ConfirmSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ConfirmSaleHandler _handler;

    public ConfirmSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new ConfirmSaleHandler(_saleRepository);
    }

    [Fact]
    public async Task Given_ValidPendingSale_When_Handle_Then_ShouldConfirmSale()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new ConfirmSaleCommand(saleId);
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 100.00m);
        
        _saleRepository.GetByIdAsync(saleId).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Sale confirmed successfully");
        sale.Status.Should().Be(SaleStatus.Confirmed);
        await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.Id == sale.Id));
    }

    [Fact]
    public async Task Given_NonExistingSale_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new ConfirmSaleCommand(Guid.NewGuid());
        _saleRepository.GetByIdAsync(command.SaleId).Returns((Sale?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Given_SaleWithoutItems_When_Handle_Then_ShouldThrowDomainException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new ConfirmSaleCommand(saleId);
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        _saleRepository.GetByIdAsync(saleId).Returns(sale);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<DomainException>()
            .WithMessage("Cannot confirm a sale without items");
    }
} 