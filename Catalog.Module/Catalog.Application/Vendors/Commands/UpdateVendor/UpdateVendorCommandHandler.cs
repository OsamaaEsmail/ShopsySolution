using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Commands.UpdateVendor;

public class UpdateVendorCommandHandler(IVendorService vendorService) : ICommandHandler<UpdateVendorCommand>
{
    public async Task<Result> Handle(UpdateVendorCommand request, CancellationToken ct)
    {
        return await vendorService.UpdateAsync(
            request.VendorId,
            request.VendorName,
            request.Email,
            request.PhoneNumber,
            request.VendorPicUrl,
            ct);
    }
}