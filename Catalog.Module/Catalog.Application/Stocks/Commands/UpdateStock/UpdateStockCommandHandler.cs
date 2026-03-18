using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Stocks.Commands.UpdateStock;

public class UpdateStockCommandHandler(IStockService stockService) : ICommandHandler<UpdateStockCommand>
{
    public async Task<Result> Handle(UpdateStockCommand request, CancellationToken ct)
    {
        return await stockService.UpdateAsync(
            request.StockId,
            request.Size,
            request.Color,
            request.Quantity,
            ct);
    }
}