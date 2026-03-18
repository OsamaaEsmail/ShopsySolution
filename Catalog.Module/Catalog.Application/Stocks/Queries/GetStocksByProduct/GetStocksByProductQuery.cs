using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Stocks.Queries.GetStocksByProduct;

public record GetStocksByProductQuery(Guid ProductId) : IQuery<IEnumerable<StockResponse>>;