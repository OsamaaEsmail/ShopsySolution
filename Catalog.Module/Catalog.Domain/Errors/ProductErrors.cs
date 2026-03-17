
using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Domain.Errors;

public record class ProductErrors
{
    public static readonly Error ProductNotFound =
        new("Product.ProductNotFound", "Product is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedProduct =
        new("Product.DuplicatedProduct", "Product already exists", StatusCodes.Status409Conflict);
}