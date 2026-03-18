using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(Guid? CategoryId = null) : IQuery<IEnumerable<ProductResponse>>;