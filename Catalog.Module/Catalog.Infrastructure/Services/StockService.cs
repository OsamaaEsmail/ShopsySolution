using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Infrastructure.Services;

public class StockService(CatalogDbContext context, IMapper mapper) : IStockService
{
    public async Task<Result<IEnumerable<StockResponse>>> GetByProductAsync(Guid productId, CancellationToken ct = default)
    {
        var productExists = await context.Products.AnyAsync(p => p.Id == productId, ct);

        if (!productExists)
            return Result.Failure<IEnumerable<StockResponse>>(ProductErrors.ProductNotFound);

        var stocks = await context.Stocks
            .Where(s => s.ProductId == productId)
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<StockResponse>>(stocks));
    }

    public async Task<Result<Guid>> AddAsync(Guid productId, string? size, string? color, int quantity, CancellationToken ct = default)
    {
        var productExists = await context.Products.AnyAsync(p => p.Id == productId, ct);

        if (!productExists)
            return Result.Failure<Guid>(ProductErrors.ProductNotFound);

        var stock = new Stock
        {
            ProductId = productId,
            Size = size,
            Color = color,
            QuantityAvailable = quantity
        };

        await context.Stocks.AddAsync(stock, ct);
        await context.SaveChangesAsync(ct);

        return Result.Success(stock.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string? size, string? color, int quantity, CancellationToken ct = default)
    {
        var stock = await context.Stocks.FindAsync([id], ct);

        if (stock is null)
            return Result.Failure(StockErrors.StockNotFound);

        stock.Size = size;
        stock.Color = color;
        stock.QuantityAvailable = quantity;
        stock.LastUpdated = DateTime.UtcNow;

        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}