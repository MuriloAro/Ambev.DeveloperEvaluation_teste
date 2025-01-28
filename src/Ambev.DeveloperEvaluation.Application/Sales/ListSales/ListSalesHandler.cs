using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesHandler : IRequestHandler<ListSalesQuery, ListSalesResult>
{
    private readonly ISaleRepository _saleRepository;

    public ListSalesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ListSalesResult> Handle(ListSalesQuery request, CancellationToken cancellationToken)
    {
        await ValidateQuery(request);

        var (sales, totalCount) = await _saleRepository.ListAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.StartDate,
            request.EndDate,
            cancellationToken
        );

        return new ListSalesResult
        {
            Items = sales.Select(s => new SaleDto
            {
                Id = s.Id,
                Number = s.Number,
                Date = s.Date,
                CustomerId = s.CustomerId,
                BranchId = s.BranchId,
                TotalAmount = s.TotalAmount,
                Status = s.Status
            }),
            TotalCount = totalCount,
            CurrentPage = request.Page,
            PageSize = request.PageSize
        };
    }

    private static Task ValidateQuery(ListSalesQuery query)
    {
        if (query.Page <= 0)
            throw new ValidationException("Page must be greater than zero");

        if (query.PageSize <= 0)
            throw new ValidationException("PageSize must be greater than zero");

        if (query.PageSize > 100)
            throw new ValidationException("PageSize cannot be greater than 100");

        if (query.StartDate.HasValue && query.EndDate.HasValue && query.StartDate > query.EndDate)
            throw new ValidationException("StartDate cannot be greater than EndDate");

        return Task.CompletedTask;
    }
} 