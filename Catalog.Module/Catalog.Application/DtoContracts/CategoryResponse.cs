namespace Catalog.Application.DtoContracts;

public record CategoryResponse(
    Guid Id,
    string CategoryName,
    List<SubCategoryResponse> SubCategories);