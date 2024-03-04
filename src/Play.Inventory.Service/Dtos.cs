using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Service;

public sealed record GrantItemsDto(
    [Required] Guid UserId,
    [Required] Guid CatalogItemId,
    [Required] [Range(0, 10000)] int Quantity
);

public sealed record InventoryItemDto(
    Guid CatalogItemId,
    string Name,
    string Description,
    int Quantity,
    DateTimeOffset AcquiredDate
);

public sealed record CatalogItemDto(Guid Id, string Name, string Description);
