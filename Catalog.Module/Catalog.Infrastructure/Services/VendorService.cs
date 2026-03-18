using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Infrastructure.Services;

public class VendorService(CatalogDbContext context, IMapper mapper) : IVendorService
{
    public async Task<Result<IEnumerable<VendorResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var vendors = await context.Vendors
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<VendorResponse>>(vendors));
    }

    public async Task<Result<VendorResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var vendor = await context.Vendors.FindAsync([id], ct);

        if (vendor is null)
            return Result.Failure<VendorResponse>(VendorErrors.VendorNotFound);

        return Result.Success(mapper.Map<VendorResponse>(vendor));
    }

    public async Task<Result<Guid>> CreateAsync(string vendorName, string email, string phoneNumber, string? vendorPicUrl, CancellationToken ct = default)
    {
        var duplicated = await context.Vendors.AnyAsync(v => v.Email == email, ct);

        if (duplicated)
            return Result.Failure<Guid>(VendorErrors.DuplicatedVendor);

        var vendor = new Vendor
        {
            VendorName = vendorName,
            Email = email,
            PhoneNumber = phoneNumber,
            VendorPicUrl = vendorPicUrl
        };

        await context.Vendors.AddAsync(vendor, ct);
        await context.SaveChangesAsync(ct);

        return Result.Success(vendor.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string vendorName, string email, string phoneNumber, string? vendorPicUrl, CancellationToken ct = default)
    {
        var vendor = await context.Vendors.FindAsync([id], ct);

        if (vendor is null)
            return Result.Failure(VendorErrors.VendorNotFound);

        var duplicated = await context.Vendors.AnyAsync(v => v.Email == email && v.Id != id, ct);

        if (duplicated)
            return Result.Failure(VendorErrors.DuplicatedVendor);

        vendor.VendorName = vendorName;
        vendor.Email = email;
        vendor.PhoneNumber = phoneNumber;
        vendor.VendorPicUrl = vendorPicUrl;

        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var vendor = await context.Vendors.FindAsync([id], ct);

        if (vendor is null)
            return Result.Failure(VendorErrors.VendorNotFound);

        context.Vendors.Remove(vendor);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}