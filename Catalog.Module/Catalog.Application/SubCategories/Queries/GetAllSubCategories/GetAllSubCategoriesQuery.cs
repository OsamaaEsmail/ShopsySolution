using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Queries.GetAllSubCategories;

public record GetAllSubCategoriesQuery() : IQuery<IEnumerable<SubCategoryResponse>>;