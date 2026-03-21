using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Queries.GetAllSubCategories;

public class GetAllSubCategoriesQueryHandler(ISubCategoryService subCategoryService) : IQueryHandler<GetAllSubCategoriesQuery, PaginatedList<SubCategoryResponse>>
{
    public async Task<Result<PaginatedList<SubCategoryResponse>>> Handle(GetAllSubCategoriesQuery request, CancellationToken ct)
    {
        return await subCategoryService.GetAllAsync(request.PageNumber, request.PageSize, ct);
    }
}