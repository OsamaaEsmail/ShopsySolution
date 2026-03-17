


using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;


namespace Catalog.Domain.Errors;

public record class StockErrors
{
    public static readonly Error StockNotFound =
        new("Stock.StockNotFound", "Stock is not found", StatusCodes.Status404NotFound);
}