using FluentAssertions;
using MapsterMapper;
using Mapster;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application.DtoContracts;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Order.Domain.Errors;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Services;
using Shopsy.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using OrderEntity = Order.Domain.Entities.Order;

namespace Shopsy.Tests.Order;

public class OrderServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<OrderService>> _logger;

    public OrderServiceTests()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<OrderEntity, OrderResponse>()
            .Map(dest => dest.OrderId, src => src.Id)
            .Map(dest => dest.OrderStatus, src => src.OrderStatus.ToString())
            .Map(dest => dest.Items, src => src.OrderItems)
            .Map(dest => dest.BillingAddress, src => src.BillingAddress)
            .Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
            .Map(dest => dest.Payment, src => src.Payment);

        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.OrderItemId, src => src.Id)
            .Map(dest => dest.TotalPrice, src => src.TotalPrice);

        config.NewConfig<BillingAddress, AddressResponse>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<ShippingAddress, AddressResponse>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<Payment, PaymentResponse>()
            .Map(dest => dest.PaymentId, src => src.Id);

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<OrderService>>();
    }

    private AddressResponse CreateTestAddress()
    {
        return new AddressResponse(
            Guid.Empty, "Test User", "+201234567890", "test@test.com",
            "123 Street", "Egypt", "Cairo", "Cairo", "11511");
    }

    private async Task<OrderEntity> SeedOrder(OrderDbContext context, string userId)
    {
        var order = new OrderEntity
        {
            UserId = userId,
            OrderItems = new List<OrderItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Size = "128GB",
                    Color = "Black",
                    Quantity = 2,
                    UnitPrice = 999.99m
                }
            },
            BillingAddress = new BillingAddress
            {
                FullName = "Test",
                PhoneNumber = "123",
                EmailAddress = "test@test.com",
                Address = "Street",
                Country = "Egypt",
                StateOrProvince = "Cairo",
                City = "Cairo",
                PostalOrZipCode = "11511"
            },
            ShippingAddress = new ShippingAddress
            {
                FullName = "Test",
                PhoneNumber = "123",
                EmailAddress = "test@test.com",
                Address = "Street",
                Country = "Egypt",
                StateOrProvince = "Cairo",
                City = "Cairo",
                PostalOrZipCode = "11511"
            }
        };

        order.RecalculateTotals();
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return order;
    }

    [Fact]
    public async Task CreateAsync_WithEmptyItems_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync(
            Guid.NewGuid().ToString(),
            new List<OrderItemResponse>(),
            CreateTestAddress(),
            CreateTestAddress());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.EmptyOrder);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingOrder_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var userId = Guid.NewGuid().ToString();
        var order = await SeedOrder(context, userId);

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(order.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(userId);
        result.Value.TotalAmount.Should().Be(1999.98m);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingOrder_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.OrderNotFound);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithValidStatus_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var order = await SeedOrder(context, Guid.NewGuid().ToString());

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateStatusAsync(order.Id, "Confirmed");

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = await context.Orders.FindAsync(order.Id);
        updated!.OrderStatus.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithInvalidStatus_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var order = await SeedOrder(context, Guid.NewGuid().ToString());

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateStatusAsync(order.Id, "InvalidStatus");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.InvalidStatus);
    }

    [Fact]
    public async Task UpdateStatusAsync_CancelledOrder_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var order = await SeedOrder(context, Guid.NewGuid().ToString());

        order.OrderStatus = OrderStatus.Cancelled;
        await context.SaveChangesAsync();

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateStatusAsync(order.Id, "Confirmed");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.OrderAlreadyCancelled);
    }

    [Fact]
    public async Task UpdateStatusAsync_DeliveredOrder_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var order = await SeedOrder(context, Guid.NewGuid().ToString());

        order.OrderStatus = OrderStatus.Delivered;
        await context.SaveChangesAsync();

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateStatusAsync(order.Id, "Confirmed");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.OrderAlreadyDelivered);
    }

    [Fact]
    public async Task UpdateStatusAsync_NonExistingOrder_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateStatusAsync(Guid.NewGuid(), "Confirmed");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.OrderNotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingOrder_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var order = new OrderEntity
        {
            UserId = Guid.NewGuid().ToString(),
            TotalQuantity = 2,
            TotalAmount = 1999.98m
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(order.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deleted = await context.Orders.FindAsync(order.Id);
        deleted.Should().BeNull();
    }
    [Fact]
    public async Task DeleteAsync_NonExistingOrder_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.OrderNotFound);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsUserOrders()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var userId = Guid.NewGuid().ToString();

        await SeedOrder(context, userId);
        await SeedOrder(context, userId);

        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByUserIdAsync(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetByUserIdAsync_NoOrders_ReturnsEmptyList()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var service = new OrderService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByUserIdAsync(Guid.NewGuid().ToString());

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}