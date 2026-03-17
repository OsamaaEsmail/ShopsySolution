


using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Domain.Errors;

public record class SubCategoryErrors
{
    public static readonly Error SubCategoryNotFound =
        new("SubCategory.SubCategoryNotFound", "SubCategory is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedSubCategory =
        new("SubCategory.DuplicatedSubCategory", "SubCategory already exists", StatusCodes.Status409Conflict);
}