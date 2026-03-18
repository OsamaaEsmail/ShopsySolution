using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryService categoryService) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        return await categoryService.CreateAsync(request.CategoryName, ct);
    }
}