using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Queries.GetVendorById;

public class GetVendorByIdQueryHandler(IVendorService vendorService) : IQueryHandler<GetVendorByIdQuery, VendorResponse>
{
    public async Task<Result<VendorResponse>> Handle(GetVendorByIdQuery request, CancellationToken ct)
    {
        return await vendorService.GetByIdAsync(request.VendorId, ct);
    }
}