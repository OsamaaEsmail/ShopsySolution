using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : ICommand;