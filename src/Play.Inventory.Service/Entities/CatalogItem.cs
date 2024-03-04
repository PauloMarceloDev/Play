using Play.Common;

namespace Play.Inventory.Service.Entities;

public sealed class CatalogItem : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
