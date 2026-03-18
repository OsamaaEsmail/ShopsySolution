using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Commands.CreateVendor;

public record CreateVendorCommand(
    string VendorName,
    string Email,
    string PhoneNumber,
    string? VendorPicUrl) : ICommand<Guid>;