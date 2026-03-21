using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Queries.GetAllVendors;

public record GetAllVendorsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PaginatedList<VendorResponse>>;