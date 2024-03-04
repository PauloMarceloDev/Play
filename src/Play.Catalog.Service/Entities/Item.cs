using Play.Common;

namespace Play.Catalog.Service.Entities;

public sealed class Item : IEntity
{
    public Guid Id { get; set; }

    // @TODO: Make the Name unique and indexed in datastore.
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
