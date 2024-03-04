using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public sealed class CatalogItemDeletedConsumer(
    IRepository<CatalogItem> catalogItemRepository,
    IRepository<InventoryItem> inventoryItemRepository
) : IConsumer<CatalogItemDeleted>
{
    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var message = context.Message;

        var item = await catalogItemRepository.GetAsync(message.ItemId, context.CancellationToken);
        if (item is null)
        {
            return;
        }

        await inventoryItemRepository.RemoveAsync(
            inventoryItem => inventoryItem.CatalogItemId == item.Id,
            context.CancellationToken
        );
        await catalogItemRepository.RemoveAsync(item.Id, context.CancellationToken);
    }
}
