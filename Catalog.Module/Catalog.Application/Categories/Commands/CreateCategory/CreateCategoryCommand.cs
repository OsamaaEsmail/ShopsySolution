using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string CategoryName) : ICommand<Guid>;