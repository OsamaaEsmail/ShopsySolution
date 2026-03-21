using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Sales.Domain.Errors;

public record class SaleErrors
{
    public static readonly Error SaleNotFound =
        new("Sale.SaleNotFound", "Sale is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedSale =
        new("Sale.DuplicatedSale", "Sale with this name already exists", StatusCodes.Status409Conflict);

    public static readonly Error InvalidDateRange =
        new("Sale.InvalidDateRange", "End date must be after start date", StatusCodes.Status400BadRequest);

    public static readonly Error SaleExpired =
        new("Sale.SaleExpired", "Sale has already expired", StatusCodes.Status400BadRequest);
}