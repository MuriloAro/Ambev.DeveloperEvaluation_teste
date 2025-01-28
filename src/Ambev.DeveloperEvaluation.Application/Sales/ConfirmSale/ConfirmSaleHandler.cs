using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

public class ConfirmSaleHandler : IRequestHandler<ConfirmSaleCommand, ConfirmSaleResult>
{
    private readonly ISaleRepository _saleRepository;

    public ConfirmSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ConfirmSaleResult> Handle(ConfirmSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.SaleId} not found");

        sale.Confirm();
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new ConfirmSaleResult 
        { 
            Success = true,
            Message = "Sale confirmed successfully"
        };
    }
} 