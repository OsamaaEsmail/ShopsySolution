using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Commands.UpdateVendor;

public record UpdateVendorCommand(
    Guid VendorId,
    string VendorName,
    string Email,
    string PhoneNumber,
    string? VendorPicUrl) : ICommand;