using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Given_ValidParameters_When_CreateSale_Then_ShouldInitializeCorrectly()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        // Act
        var sale = new Sale(customerId, branchId);

        // Assert
        sale.CustomerId.Should().Be(customerId);
        sale.BranchId.Should().Be(branchId);
        sale.Status.Should().Be(SaleStatus.Pending);
        sale.Number.Should().NotBeNullOrEmpty();
        sale.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        sale.DomainEvents.Should().ContainSingle(e => e is SaleCreatedEvent);
    }

    [Fact]
    public void Given_ValidItem_When_AddItem_Then_ShouldCalculateTotalCorrectly()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();
        var quantity = 5;
        var unitPrice = 10.00m;

        // Act
        sale.AddItem(productId, quantity, unitPrice);

        // Assert
        sale.Items.Should().HaveCount(1);
        sale.TotalAmount.Should().Be(45.00m); // 5 * 10 - 10% discount
        sale.DomainEvents.Should().Contain(e => e is SaleModifiedEvent);
    }

    [Theory]
    [InlineData(3, 0)]    // No discount
    [InlineData(4, 0.10)] // 10% discount
    [InlineData(10, 0.20)] // 20% discount
    public void Given_DifferentQuantities_When_AddItem_Then_ShouldApplyCorrectDiscount(int quantity, decimal expectedDiscount)
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var unitPrice = 100.00m;
        var expectedTotal = quantity * unitPrice * (1 - expectedDiscount);

        // Act
        sale.AddItem(Guid.NewGuid(), quantity, unitPrice);

        // Assert
        sale.TotalAmount.Should().Be(expectedTotal);
    }

    [Fact]
    public void Given_QuantityAboveLimit_When_AddItem_Then_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        var act = () => sale.AddItem(Guid.NewGuid(), 21, 10.00m);
        act.Should().Throw<DomainException>()
           .WithMessage("Cannot sell more than 20 identical items");
    }

    [Fact]
    public void Given_PendingSale_When_Cancel_Then_ShouldUpdateStatusAndRaiseEvents()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var reason = "Customer request";

        // Act
        sale.Cancel(reason);

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.DomainEvents.Should().Contain(e => e is SaleCancelledEvent);
        sale.DomainEvents.Should().Contain(e => e is SaleModifiedEvent);
    }

    [Fact]
    public void Given_CompletedSale_When_Cancel_Then_ShouldThrowException()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 10.00m);
        sale.Confirm();
        sale.Complete();

        // Act & Assert
        var act = () => sale.Cancel("Any reason");
        act.Should().Throw<DomainException>()
           .WithMessage("Completed sales cannot be cancelled");
    }
} 