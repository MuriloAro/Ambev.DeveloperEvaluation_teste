using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CancelSaleHandler(_saleRepository);
    }

    [Fact]
    public async Task Given_ValidPendingSale_When_Handle_Then_ShouldCancelSale()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId, "Customer request");
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 100.00m);
        
        _saleRepository.GetByIdAsync(saleId).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Sale cancelled successfully");
        sale.Status.Should().Be(SaleStatus.Cancelled);
        await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.Id == sale.Id));
    }

    [Fact]
    public async Task Given_NonExistingSale_When_Handle_Then_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.NewGuid(), "Any reason");
        _saleRepository.GetByIdAsync(command.SaleId).Returns((Sale?)null);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Given_CompletedSale_When_Handle_Then_ShouldThrowDomainException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId, "Any reason");
        
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 100.00m);
        sale.Confirm();
        sale.Complete();
        
        _saleRepository.GetByIdAsync(saleId).Returns(sale);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<DomainException>()
            .WithMessage("Completed sales cannot be cancelled");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Given_EmptyReason_When_Handle_Then_ShouldThrowValidationException(string? reason)
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.NewGuid(), reason!);

        // Act & Assert
        var action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>()
            .WithMessage("Cancellation reason is required");
    }
} 