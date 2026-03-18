using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Commands.DeleteVendor;

public record DeleteVendorCommand(Guid VendorId) : ICommand;