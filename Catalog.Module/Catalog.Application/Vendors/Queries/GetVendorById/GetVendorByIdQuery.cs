using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Vendors.Queries.GetVendorById;

public record GetVendorByIdQuery(Guid VendorId) : IQuery<VendorResponse>;