using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid CategoryId, string CategoryName) : ICommand;