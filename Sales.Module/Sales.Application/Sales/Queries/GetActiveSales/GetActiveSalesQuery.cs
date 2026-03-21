using Sales.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Queries.GetActiveSales;

public record GetActiveSalesQuery() : IQuery<IEnumerable<SaleResponse>>;