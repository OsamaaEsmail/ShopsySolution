



using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Domain.Errors;

public record class VendorErrors
{
    public static readonly Error VendorNotFound =
        new("Vendor.VendorNotFound", "Vendor is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedVendor =
        new("Vendor.DuplicatedVendor", "Vendor already exists", StatusCodes.Status409Conflict);
}