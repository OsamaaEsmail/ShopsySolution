using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Queries.GetAllVendors;

public record GetAllVendorsQuery() : IQuery<IEnumerable<VendorResponse>>;