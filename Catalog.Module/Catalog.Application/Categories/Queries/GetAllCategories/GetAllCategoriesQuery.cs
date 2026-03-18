using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery() : IQuery<IEnumerable<CategoryResponse>>;