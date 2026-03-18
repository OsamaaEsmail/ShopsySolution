using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Queries.GetSubCategoriesByCategory;

public class GetSubCategoriesByCategoryQueryHandler(ISubCategoryService subCategoryService) : IQueryHandler<GetSubCategoriesByCategoryQuery, IEnumerable<SubCategoryResponse>>
{
    public async Task<Result<IEnumerable<SubCategoryResponse>>> Handle(GetSubCategoriesByCategoryQuery request, CancellationToken ct)
    {
        return await subCategoryService.GetByCategoryAsync(request.CategoryId, ct);
    }
}