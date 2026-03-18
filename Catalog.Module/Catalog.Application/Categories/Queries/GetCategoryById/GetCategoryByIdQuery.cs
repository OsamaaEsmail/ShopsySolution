using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<CategoryResponse>;