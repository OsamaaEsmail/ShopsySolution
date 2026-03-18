using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Stocks.Queries.GetStocksByProduct;

public class GetStocksByProductQueryHandler(IStockService stockService) : IQueryHandler<GetStocksByProductQuery, IEnumerable<StockResponse>>
{
    public async Task<Result<IEnumerable<StockResponse>>> Handle(GetStocksByProductQuery request, CancellationToken ct)
    {
        return await stockService.GetByProductAsync(request.ProductId, ct);
    }
}