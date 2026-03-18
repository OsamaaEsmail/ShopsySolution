using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Stocks.Commands.AddStock;

public class AddStockCommandHandler(IStockService stockService) : ICommandHandler<AddStockCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddStockCommand request, CancellationToken ct)
    {
        return await stockService.AddAsync(
            request.ProductId,
            request.Size,
            request.Color,
            request.Quantity,
            ct);
    }
}