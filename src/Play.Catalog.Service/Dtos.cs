using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service;

public sealed record ItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    DateTimeOffset CreatedDate
);

public sealed record CreateItemDto(
    [Required] string Name,
    [Required] string Description,
    [Required] [Range(1, 100000)] decimal Price
);

public sealed record UpdateItemDto(
    [Required] string Name,
    [Required] string Description,
    [Required] [Range(1, 100000)] decimal Price
);
