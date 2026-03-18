using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Commands.CreateVendor;

public class CreateVendorCommandHandler(IVendorService vendorService) : ICommandHandler<CreateVendorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateVendorCommand request, CancellationToken ct)
    {
        return await vendorService.CreateAsync(
            request.VendorName,
            request.Email,
            request.PhoneNumber,
            request.VendorPicUrl,
            ct);
    }
}