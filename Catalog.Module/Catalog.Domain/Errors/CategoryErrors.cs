



using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Domain.Errors;

public record class CategoryErrors
{
    public static readonly Error CategoryNotFound =
        new("Category.CategoryNotFound", "Category is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedCategory =
        new("Category.DuplicatedCategory", "Category already exists", StatusCodes.Status409Conflict);
}