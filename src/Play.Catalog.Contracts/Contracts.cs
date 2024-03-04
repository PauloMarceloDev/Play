namespace Play.Catalog.Contracts;

public sealed record CatalogItemCreated(Guid ItemId, string Name, string Description);

public sealed record CatalogItemUpdated(Guid ItemId, string Name, string Description);

public sealed record CatalogItemDeleted(Guid ItemId);