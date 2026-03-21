using Sales.Application.DtoContracts;
using Sales.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Queries.GetSaleById;

public class GetSaleByIdQueryHandler(ISaleService saleService) : IQueryHandler<GetSaleByIdQuery, SaleResponse>
{
    public async Task<Result<SaleResponse>> Handle(GetSaleByIdQuery request, CancellationToken ct)
    {
        return await saleService.GetByIdAsync(request.SaleId, ct);
    }
}