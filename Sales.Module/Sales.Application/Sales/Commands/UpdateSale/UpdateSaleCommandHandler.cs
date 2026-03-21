using Sales.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Commands.UpdateSale;

public class UpdateSaleCommandHandler(ISaleService saleService) : ICommandHandler<UpdateSaleCommand>
{
    public async Task<Result> Handle(UpdateSaleCommand request, CancellationToken ct)
    {
        return await saleService.UpdateAsync(
            request.SaleId,
            request.SaleName,
            request.DiscountPercentage,
            request.StartDate,
            request.EndDate,
            request.SaleImage,
            ct);
    }
}