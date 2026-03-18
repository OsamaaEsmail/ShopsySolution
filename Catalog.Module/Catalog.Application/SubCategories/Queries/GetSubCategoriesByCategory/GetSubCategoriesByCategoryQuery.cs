using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Queries.GetSubCategoriesByCategory;

public record GetSubCategoriesByCategoryQuery(Guid CategoryId) : IQuery<IEnumerable<SubCategoryResponse>>;