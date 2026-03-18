using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<ProductResponse>;