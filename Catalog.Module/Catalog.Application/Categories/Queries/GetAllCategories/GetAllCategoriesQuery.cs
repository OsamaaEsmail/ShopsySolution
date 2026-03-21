using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PaginatedList<CategoryResponse>>;