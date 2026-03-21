using Sales.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Commands.DeleteSale;

public class DeleteSaleCommandHandler(ISaleService saleService) : ICommandHandler<DeleteSaleCommand>
{
    public async Task<Result> Handle(DeleteSaleCommand request, CancellationToken ct)
    {
        return await saleService.DeleteAsync(request.SaleId, ct);
    }
}