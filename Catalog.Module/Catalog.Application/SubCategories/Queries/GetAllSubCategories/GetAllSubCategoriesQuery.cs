using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Queries.GetAllSubCategories;

public record GetAllSubCategoriesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PaginatedList<SubCategoryResponse>>;