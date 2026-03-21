using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 10, Guid? CategoryId = null) : IQuery<PaginatedList<ProductResponse>>;