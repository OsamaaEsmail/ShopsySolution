using Sales.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Commands.CreateSale;

public class CreateSaleCommandHandler(ISaleService saleService) : ICommandHandler<CreateSaleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSaleCommand request, CancellationToken ct)
    {
        return await saleService.CreateAsync(
            request.SaleName,
            request.SaleType,
            request.DiscountPercentage,
            request.StartDate,
            request.EndDate,
            request.SaleImage,
            request.Products,
            ct);
    }
}