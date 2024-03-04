namespace Play.Inventory.Service.Clients;

public sealed class CatalogClient(HttpClient httpClient)
{
    public async Task<IReadOnlyCollection<CatalogItemDto>?> GetCatalogItemsAsync(
        CancellationToken cancellationToken
    ) =>
        await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>(
            "/items",
            cancellationToken: cancellationToken
        );
}
