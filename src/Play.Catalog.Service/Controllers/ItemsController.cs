using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Play.Catalog.Service.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
[Route("items")]
public sealed class ItemsController(IRepository<Item> itemsRepository) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary ="Fetches the list of items.")]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>),StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync(CancellationToken cancellationToken) => 
        Ok((await itemsRepository.GetAllAsync(cancellationToken)).Select(i => i.AsDto()));

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary ="Retrieves an item based on its ID.")]
    [ProducesResponseType(typeof(ItemDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var item = await itemsRepository.GetAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item.AsDto());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemDto),StatusCodes.Status200OK)]
    [SwaggerOperation(Summary ="Creates an item.")]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto request, CancellationToken cancellationToken)
    {
        var item = request.AsEntity(Guid.NewGuid(), DateTimeOffset.Now);
        await itemsRepository.CreateAsync(item, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary ="Modifies an item based on its ID.")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto request, CancellationToken cancellationToken)
    {
        var existingItem = await itemsRepository.GetAsync(id, cancellationToken);
        if (existingItem is null)
        {
            return NotFound("Invalid item ID.");
        }

        await itemsRepository.UpdateAsync(request.AsEntity(existingItem.Id, existingItem.CreatedDate), 
            cancellationToken);
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(Summary ="Removes an item based on its ID.")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var item = await itemsRepository.GetAsync(id, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        await itemsRepository.RemoveAsync(id, cancellationToken);
        
        return NoContent();
    }
}