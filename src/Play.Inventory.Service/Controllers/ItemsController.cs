using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Play.Inventory.Service.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
[Route("items")]
public sealed class ItemsController(
    IRepository<InventoryItem> itemsRepository,
    IRepository<CatalogItem> catalogItemRepository
) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Fetches the list of items in inventory by UserId.")]
    [ProducesResponseType(typeof(IEnumerable<InventoryItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAsync(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }

        var itemsFromCatalog = await catalogItemRepository.GetAllAsync(cancellationToken);

        var inventoryItemDtos = (
            await itemsRepository.GetAllAsync(i => i.UserId == userId, cancellationToken)
        ).Select(inventoryItem =>
        {
            var itemCatalogById = itemsFromCatalog!.Single(itemFromCatalog =>
                itemFromCatalog.Id == inventoryItem.CatalogItemId
            );
            return inventoryItem.AsDto(itemCatalogById.Name, itemCatalogById.Description);
        });

        return Ok(inventoryItemDtos);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create item or update a item quantity in the user's inventory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> PostAsync(
        GrantItemsDto request,
        CancellationToken cancellationToken
    )
    {
        var inventoryItem = await itemsRepository.GetAsync(
            item => item.UserId == request.UserId && item.CatalogItemId == request.CatalogItemId,
            cancellationToken
        );
        var isFirstItemInUserInventory = inventoryItem is null;

        if (isFirstItemInUserInventory)
        {
            inventoryItem = request.AsEntity(DateTimeOffset.UtcNow);
            await itemsRepository.CreateAsync(inventoryItem, cancellationToken);
        }
        else
        {
            inventoryItem!.Quantity = request.Quantity;
            await itemsRepository.UpdateAsync(inventoryItem, cancellationToken);
        }

        return Ok();
    }
}
