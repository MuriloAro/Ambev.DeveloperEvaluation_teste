using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for processing sales listing commands
/// </summary>
public sealed class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListSalesHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the ListSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<ListSalesHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the sales listing command
    /// </summary>
    /// <param name="request">The listing command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the listing operation</returns>
    public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing sales. Page: {Page}, PageSize: {PageSize}", request.Page, request.PageSize);

        var (sales, totalCount) = await _saleRepository.ListAsync(
            page: request.Page,
            pageSize: request.PageSize,
            status: request.Status,
            startDate: request.StartDate,
            endDate: request.EndDate
        );

        var result = new ListSalesResult
        {
            Items = _mapper.Map<ICollection<SaleItemResult>>(sales),
            TotalCount = totalCount,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };

        _logger.LogInformation("Found {Count} sales", totalCount);

        return result;
    }
} 