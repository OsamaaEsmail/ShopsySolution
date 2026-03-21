using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Domain.Errors;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Services;
using Shopsy.Tests.Helpers;
using OrderEntity = Order.Domain.Entities.Order;

namespace Shopsy.Tests.Order;

public class PaymentServiceTests
{
    private readonly Mock<ILogger<PaymentService>> _logger;

    public PaymentServiceTests()
    {
        _logger = new Mock<ILogger<PaymentService>>();
    }

    private async Task<OrderEntity> SeedOrder(OrderDbContext context, string userId, decimal totalAmount)
    {
        var order = new OrderEntity
        {
            UserId = userId,
            TotalQuantity = 2,
            TotalAmount = totalAmount
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return order;
    }

    [Fact]
    public async Task CreateAsync_WithValidOrder_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var userId = Guid.NewGuid().ToString();
        var order = await SeedOrder(context, userId, 1999.98m);

        var service = new PaymentService(context, _logger.Object);

        // Act
        var result = await service.CreateAsync(order.Id, userId, "CreditCard");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var payment = await context.Payments.FindAsync(result.Value);
        payment.Should().NotBeNull();
        payment!.Amount.Should().Be(1999.98m);
        payment.PaymentMethod.Should().Be("CreditCard");
        payment.PaymentStatus.Should().Be("Completed");
    }

    [Fact]
    public async Task CreateAsync_WithNonExistingOrder_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var service = new PaymentService(context, _logger.Object);

        // Act
        var result = await service.CreateAsync(Guid.NewGuid(), Guid.NewGuid().ToString(), "CreditCard");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.OrderNotFound);
    }

    [Fact]
    public async Task CreateAsync_WithExistingPayment_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var userId = Guid.NewGuid().ToString();
        var order = await SeedOrder(context, userId, 1999.98m);

        var service = new PaymentService(context, _logger.Object);

        // First payment
        await service.CreateAsync(order.Id, userId, "CreditCard");

        // Act - Second payment
        var result = await service.CreateAsync(order.Id, userId, "PayPal");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PaymentErrors.PaymentAlreadyExists);
    }

    [Fact]
    public async Task CreateAsync_SetsCorrectAmount()
    {
        // Arrange
        var context = TestDbContextFactory.CreateOrderContext();
        var userId = Guid.NewGuid().ToString();
        var order = await SeedOrder(context, userId, 500.00m);

        var service = new PaymentService(context, _logger.Object);

        // Act
        var result = await service.CreateAsync(order.Id, userId, "DebitCard");

        // Assert
        result.IsSuccess.Should().BeTrue();

        var payment = await context.Payments.FindAsync(result.Value);
        payment!.Amount.Should().Be(500.00m);
    }
}