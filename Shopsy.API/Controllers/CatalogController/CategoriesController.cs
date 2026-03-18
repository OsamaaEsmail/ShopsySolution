using Asp.Versioning;
using Catalog.Application.Categories.Commands.CreateCategory;
using Catalog.Application.Categories.Commands.DeleteCategory;
using Catalog.Application.Categories.Commands.UpdateCategory;
using Catalog.Application.Categories.Queries.GetAllCategories;
using Catalog.Application.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.API.Controllers.CatalogController;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllCategoriesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateCategoryCommand(id, request.CategoryName), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteCategoryCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateCategoryRequest(string CategoryName);