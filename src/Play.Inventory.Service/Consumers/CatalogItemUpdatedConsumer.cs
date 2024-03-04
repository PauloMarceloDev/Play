using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public sealed class CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
    : IConsumer<CatalogItemUpdated>
{
    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var message = context.Message;

        var item = await repository.GetAsync(message.ItemId, context.CancellationToken);
        if (item is null)
        {
            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };
            await repository.CreateAsync(item, context.CancellationToken);
            return;
        }

        item = new CatalogItem
        {
            Id = message.ItemId,
            Name = message.Name,
            Description = message.Description
        };
        await repository.UpdateAsync(item, context.CancellationToken);
    }
}
