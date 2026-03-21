using Sales.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Queries.GetActiveSales;

public record GetActiveSalesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PaginatedList<SaleResponse>>;