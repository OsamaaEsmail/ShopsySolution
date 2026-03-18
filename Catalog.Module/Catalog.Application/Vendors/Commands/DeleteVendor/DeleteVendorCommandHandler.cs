using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Commands.DeleteVendor;

public class DeleteVendorCommandHandler(IVendorService vendorService) : ICommandHandler<DeleteVendorCommand>
{
    public async Task<Result> Handle(DeleteVendorCommand request, CancellationToken ct)
    {
        return await vendorService.DeleteAsync(request.VendorId, ct);
    }
}