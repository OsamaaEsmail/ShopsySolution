using Sales.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Queries.GetSaleById;

public record GetSaleByIdQuery(Guid SaleId) : IQuery<SaleResponse>;