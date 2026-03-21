using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Queries.GetAllVendors;

public class GetAllVendorsQueryHandler(IVendorService vendorService) : IQueryHandler<GetAllVendorsQuery, PaginatedList<VendorResponse>>
{
    public async Task<Result<PaginatedList<VendorResponse>>> Handle(GetAllVendorsQuery request, CancellationToken ct)
    {
        return await vendorService.GetAllAsync(request.PageNumber, request.PageSize, ct);
    }
}