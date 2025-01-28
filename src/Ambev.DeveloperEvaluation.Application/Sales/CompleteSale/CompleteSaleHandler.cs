using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

public class CompleteSaleHandler : IRequestHandler<CompleteSaleCommand, CompleteSaleResult>
{
    private readonly ISaleRepository _saleRepository;

    public CompleteSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<CompleteSaleResult> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.SaleId} not found");

        sale.Complete();
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new CompleteSaleResult 
        { 
            Success = true,
            Message = "Sale completed successfully"
        };
    }
} 