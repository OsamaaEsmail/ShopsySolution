using Sales.Application.DtoContracts;
using Sales.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Queries.GetActiveSales;

public class GetActiveSalesQueryHandler(ISaleService saleService) : IQueryHandler<GetActiveSalesQuery, PaginatedList<SaleResponse>>
{
    public async Task<Result<PaginatedList<SaleResponse>>> Handle(GetActiveSalesQuery request, CancellationToken ct)
    {
        return await saleService.GetActiveSalesAsync(request.PageNumber, request.PageSize, ct);
    }
}