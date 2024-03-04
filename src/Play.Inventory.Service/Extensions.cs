using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service;

public static class Extensions
{
    public static InventoryItemDto AsDto(
        this InventoryItem entity,
        string name,
        string description
    ) => new(entity.CatalogItemId, name, description, entity.Quantity, entity.AcquiredDate);

    public static InventoryItem AsEntity(this GrantItemsDto dto, DateTimeOffset acquiredDate) =>
        new()
        {
            CatalogItemId = dto.CatalogItemId,
            UserId = dto.UserId,
            Quantity = dto.Quantity,
            AcquiredDate = acquiredDate
        };
}
