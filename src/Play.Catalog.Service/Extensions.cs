using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service;

public static class Extensions
{
    public static ItemDto AsDto(this Item entity) =>
        new(entity.Id, entity.Name, entity.Description, entity.Price, entity.CreatedDate);

    public static Item AsEntity(this CreateItemDto dto, Guid id, DateTimeOffset createdDate) =>
        new()
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CreatedDate = createdDate
        };

    public static Item AsEntity(this UpdateItemDto dto, Guid id, DateTimeOffset createdDate) =>
        new()
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CreatedDate = createdDate
        };
}
