using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.ListSales;

public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ListSalesHandler _handler;

    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new ListSalesHandler(_saleRepository);
    }

    [Fact]
    public async Task Given_ValidQuery_When_Handle_Then_ShouldReturnPagedSales()
    {
        // Arrange
        var query = new ListSalesQuery
        {
            Page = 1,
            PageSize = 10,
            Status = SaleStatus.Pending,
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow
        };

        var sales = new List<Sale>
        {
            CreateSale(SaleStatus.Pending),
            CreateSale(SaleStatus.Pending)
        };

        var totalCount = 2;
        _saleRepository.ListAsync(
            Arg.Any<int>(), 
            Arg.Any<int>(), 
            Arg.Any<SaleStatus?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<DateTime?>()
        ).Returns((sales, totalCount));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(totalCount);
        result.CurrentPage.Should().Be(query.Page);
        result.TotalPages.Should().Be(1);
        
        await _saleRepository.Received(1).ListAsync(
            query.Page,
            query.PageSize,
            query.Status,
            query.StartDate,
            query.EndDate,
            Arg.Any<CancellationToken>()
        );
    }

    [Theory]
    [InlineData(0, 10)]  // Invalid page
    [InlineData(1, 0)]   // Invalid pageSize
    [InlineData(1, 101)] // PageSize too large
    public async Task Given_InvalidPagination_When_Handle_Then_ShouldThrowValidationException(int page, int pageSize)
    {
        // Arrange
        var query = new ListSalesQuery { Page = page, PageSize = pageSize };

        // Act & Assert
        var action = async () => await _handler.Handle(query, CancellationToken.None);
        await action.Should().ThrowAsync<ValidationException>();
    }

    private static Sale CreateSale(SaleStatus status)
    {
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.AddItem(Guid.NewGuid(), 1, 100.00m);
        
        if (status == SaleStatus.Confirmed || status == SaleStatus.Completed)
            sale.Confirm();
            
        if (status == SaleStatus.Completed)
            sale.Complete();
            
        if (status == SaleStatus.Cancelled)
            sale.Cancel("Test cancellation");

        return sale;
    }
} 