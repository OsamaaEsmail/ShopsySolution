using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Infrastructure.Services;

public class VendorService(CatalogDbContext context, IMapper mapper, ILogger<VendorService> logger) : IVendorService
{
    public async Task<Result<PaginatedList<VendorResponse>>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Getting vendors, Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var query = context.Vendors.AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var vendors = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var mapped = mapper.Map<List<VendorResponse>>(vendors);

        logger.LogInformation("Found {Count} vendors on page {PageNumber}", mapped.Count, pageNumber);

        return Result.Success(new PaginatedList<VendorResponse>(mapped, pageNumber, totalCount, pageSize));
    }
    public async Task<Result<VendorResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Getting vendor by ID: {VendorId}", id);

        var vendor = await context.Vendors.FindAsync([id], ct);

        if (vendor is null)
        {
            logger.LogWarning("Vendor not found: {VendorId}", id);
            return Result.Failure<VendorResponse>(VendorErrors.VendorNotFound);
        }

        return Result.Success(mapper.Map<VendorResponse>(vendor));
    }

    public async Task<Result<Guid>> CreateAsync(string vendorName, string email, string phoneNumber, string? vendorPicUrl, CancellationToken ct = default)
    {
        logger.LogInformation("Creating vendor: {VendorName}", vendorName);

        var duplicated = await context.Vendors.AnyAsync(v => v.Email == email, ct);

        if (duplicated)
        {
            logger.LogWarning("Duplicate vendor email: {Email}", email);
            return Result.Failure<Guid>(VendorErrors.DuplicatedVendor);
        }

        var vendor = new Vendor
        {
            VendorName = vendorName,
            Email = email,
            PhoneNumber = phoneNumber,
            VendorPicUrl = vendorPicUrl
        };

        await context.Vendors.AddAsync(vendor, ct);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Vendor created: {VendorId} - {VendorName}", vendor.Id, vendorName);

        return Result.Success(vendor.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string vendorName, string email, string phoneNumber, string? vendorPicUrl, CancellationToken ct = default)
    {
        logger.LogInformation("Updating vendor: {VendorId}", id);

        var vendor = await context.Vendors.FindAsync([id], ct);

        if (vendor is null)
        {
            logger.LogWarning("Vendor not found for update: {VendorId}", id);
            return Result.Failure(VendorErrors.VendorNotFound);
        }

        var duplicated = await context.Vendors.AnyAsync(v => v.Email == email && v.Id != id, ct);

        if (duplicated)
        {
            logger.LogWarning("Duplicate vendor email on update: {Email}", email);
            return Result.Failure(VendorErrors.DuplicatedVendor);
        }

        vendor.VendorName = vendorName;
        vendor.Email = email;
        vendor.PhoneNumber = phoneNumber;
        vendor.VendorPicUrl = vendorPicUrl;

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Vendor updated: {VendorId}", id);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Deleting vendor: {VendorId}", id);

        var vendor = await context.Vendors.FindAsync([id], ct);

        if (vendor is null)
        {
            logger.LogWarning("Vendor not found for delete: {VendorId}", id);
            return Result.Failure(VendorErrors.VendorNotFound);
        }

        context.Vendors.Remove(vendor);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Vendor deleted: {VendorId}", id);

        return Result.Success();
    }
}