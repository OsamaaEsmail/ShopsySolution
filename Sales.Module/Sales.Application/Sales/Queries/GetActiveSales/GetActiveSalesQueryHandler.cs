using Sales.Application.DtoContracts;
using Sales.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Queries.GetActiveSales;

public class GetActiveSalesQueryHandler(ISaleService saleService) : IQueryHandler<GetActiveSalesQuery, IEnumerable<SaleResponse>>
{
    public async Task<Result<IEnumerable<SaleResponse>>> Handle(GetActiveSalesQuery request, CancellationToken ct)
    {
        return await saleService.GetActiveSalesAsync(ct);
    }
}