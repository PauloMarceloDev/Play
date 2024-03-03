namespace Play.Inventory.Service;

public sealed record GrantItemsDto(Guid UserId, Guid  CatalogItemId, int Quantity);

public sealed record InventoryItemDto(Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);