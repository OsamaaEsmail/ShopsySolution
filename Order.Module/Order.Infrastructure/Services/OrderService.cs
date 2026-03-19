using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Order.Application.DtoContracts;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Domain.Errors;
using Order.Infrastructure.Persistence;
using Shopsy.BuildingBlocks.Abstractions;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Infrastructure.Services;

public class OrderService(OrderDbContext context, IMapper mapper) : IOrderService
{
    public async Task<Result<OrderResponse>> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
            return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);

        return Result.Success(mapper.Map<OrderResponse>(order));
    }

    public async Task<Result<IEnumerable<OrderResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var orders = await context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payment)
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<OrderResponse>>(orders));
    }

    public async Task<Result<Guid>> CreateAsync(string userId, List<OrderItemResponse> items, AddressResponse billingAddress, AddressResponse shippingAddress, CancellationToken ct = default)
    {
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
        await context.SaveChangesAsync(ct);

        return Result.Success(order.Id);
    }

    public async Task<Result> UpdateStatusAsync(Guid orderId, string status, CancellationToken ct = default)
    {
        var order = await context.Orders.FindAsync([orderId], ct);

        if (order is null)
            return Result.Failure(OrderErrors.OrderNotFound);

        if (order.OrderStatus == OrderStatus.Cancelled)
            return Result.Failure(OrderErrors.OrderAlreadyCancelled);

        if (order.OrderStatus == OrderStatus.Delivered)
            return Result.Failure(OrderErrors.OrderAlreadyDelivered);

        if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            return Result.Failure(OrderErrors.InvalidStatus);

        order.OrderStatus = newStatus;
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.BillingAddress)
            .Include(o => o.ShippingAddress)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
            return Result.Failure(OrderErrors.OrderNotFound);

        context.Orders.Remove(order);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}