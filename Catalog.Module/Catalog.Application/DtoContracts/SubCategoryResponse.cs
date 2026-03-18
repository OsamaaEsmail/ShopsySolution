namespace Catalog.Application.DtoContracts;

public record SubCategoryResponse(
    Guid Id,
    string SubCategoryName,
    Guid CategoryId);