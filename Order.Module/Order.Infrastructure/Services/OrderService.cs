using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Application.DtoContracts;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Domain.Errors;
using Order.Infrastructure.Persistence;
using Shopsy.BuildingBlocks.Abstractions;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Infrastructure.Services;

public class OrderService(OrderDbContext context, IMapper mapper, ILogger<OrderService> logger) : IOrderService
{
    public async Task<Result<OrderResponse>> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    {
        logger.LogInformation("Getting order by ID: {OrderId}", orderId);

        var order = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
        {
            logger.LogWarning("Order not found: {OrderId}", orderId);
            return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);
        }

        return Result.Success(mapper.Map<OrderResponse>(order));
    }

    public async Task<Result<IEnumerable<OrderResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        logger.LogInformation("Getting orders for user: {UserId}", userId);

        var orders = await context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payment)
            .AsNoTracking()
            .ToListAsync(ct);

        logger.LogInformation("Found {Count} orders for user: {UserId}", orders.Count, userId);

        return Result.Success(mapper.Map<IEnumerable<OrderResponse>>(orders));
    }

    public async Task<Result<Guid>> CreateAsync(string userId, List<OrderItemResponse> items, AddressResponse billingAddress, AddressResponse shippingAddress, CancellationToken ct = default)
    {
        logger.LogInformation("Creating order for user: {UserId} with {Count} items", userId, items.Count);

        if (!items.Any())
            return Result.Failure<Guid>(OrderErrors.EmptyOrder);

        var order = new OrderEntity
        {
            UserId = userId,
            OrderItems = items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Size = i.Size,
                Color = i.Color,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),
            BillingAddress = new BillingAddress
            {
                FullName = billingAddress.FullName,
                PhoneNumber = billingAddress.PhoneNumber,
                EmailAddress = billingAddress.EmailAddress,
                Address = billingAddress.Address,
                Country = billingAddress.Country,
                StateOrProvince = billingAddress.StateOrProvince,
                City = billingAddress.City,
                PostalOrZipCode = billingAddress.PostalOrZipCode
            },
            ShippingAddress = new ShippingAddress
            {
                FullName = shippingAddress.FullName,
                PhoneNumber = shippingAddress.PhoneNumber,
                EmailAddress = shippingAddress.EmailAddress,
                Address = shippingAddress.Address,
                Country = shippingAddress.Country,
                StateOrProvince = shippingAddress.StateOrProvince,
                City = shippingAddress.City,
                PostalOrZipCode = shippingAddress.PostalOrZipCode
            }
        };

        order.RecalculateTotals();

        await context.Orders.AddAsync(order, ct);

        // Deduct stock for each order item
        foreach (var item in order.OrderItems)
        {
            var sql = @"
            UPDATE [catalog].[Stocks] 
            SET QuantityAvailable = QuantityAvailable - {0}
            WHERE ProductId = {1} 
            AND ({2} IS NULL OR Color = {2}) 
            AND ({3} IS NULL OR Size = {3})
            AND QuantityAvailable >= {0}";

            var rowsAffected = await context.Database
                .ExecuteSqlRawAsync(sql, item.Quantity, item.ProductId, item.Color, item.Size);

            if (rowsAffected == 0)
            {
                logger.LogWarning("Insufficient stock for product: {ProductId}, Size: {Size}, Color: {Color}", item.ProductId, item.Size, item.Color);
                return Result.Failure<Guid>(new Error("Order.InsufficientStock", $"Insufficient stock for product {item.ProductId}", 400));
            }
        }

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Order created: {OrderId}, Total: {TotalAmount} for user: {UserId}", order.Id, order.TotalAmount, userId);

        return Result.Success(order.Id);
    }

    public async Task<Result> UpdateStatusAsync(Guid orderId, string status, CancellationToken ct = default)
    {
        logger.LogInformation("Updating order status: {OrderId} to {Status}", orderId, status);

        var order = await context.Orders.FindAsync([orderId], ct);

        if (order is null)
        {
            logger.LogWarning("Order not found: {OrderId}", orderId);
            return Result.Failure(OrderErrors.OrderNotFound);
        }

        if (order.OrderStatus == OrderStatus.Cancelled)
        {
            logger.LogWarning("Cannot update cancelled order: {OrderId}", orderId);
            return Result.Failure(OrderErrors.OrderAlreadyCancelled);
        }

        if (order.OrderStatus == OrderStatus.Delivered)
        {
            logger.LogWarning("Cannot update delivered order: {OrderId}", orderId);
            return Result.Failure(OrderErrors.OrderAlreadyDelivered);
        }

        if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
        {
            logger.LogWarning("Invalid order status: {Status}", status);
            return Result.Failure(OrderErrors.InvalidStatus);
        }

        order.OrderStatus = newStatus;
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Order status updated: {OrderId} → {Status}", orderId, newStatus);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid orderId, CancellationToken ct = default)
    {
        logger.LogInformation("Deleting order: {OrderId}", orderId);

        var order = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
        {
            logger.LogWarning("Order not found for delete: {OrderId}", orderId);
            return Result.Failure(OrderErrors.OrderNotFound);
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Order deleted: {OrderId}", orderId);

        return Result.Success();
    }
}