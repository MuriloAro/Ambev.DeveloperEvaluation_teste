using Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CompleteSale;

public class CompleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly CompleteSaleHandler _handler;

    public CompleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CompleteSaleHandler(_saleRepository);
    }

    [Fact]
    public async Task Given_ValidConfirmedSale_When_Handle_Then_ShouldCompleteSale()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CompleteSaleCommand(saleId);
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 100.00m);
        sale.Confirm();
        
        _saleRepository.GetByIdAsync(saleId).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Sale completed successfully");
        sale.Status.Should().Be(SaleStatus.Completed);
        await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.Id == sale.Id));
    }

    [Fact]
    public async Task Given_NonExistingSale_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new CompleteSaleCommand(Guid.NewGuid());
        _saleRepository.GetByIdAsync(command.SaleId).Returns((Sale?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Given_NonConfirmedSale_When_Handle_Then_ShouldThrowDomainException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CompleteSaleCommand(saleId);
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 100.00m);
        // Sale não está confirmada
        
        _saleRepository.GetByIdAsync(saleId).Returns(sale);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<DomainException>()
            .WithMessage("Only confirmed sales can be completed");
    }
} 