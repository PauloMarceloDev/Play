using Play.Identity.Service.Entities;

using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Service;

public sealed record UserDto(
    Guid Id,
    string Username,
    string Email,
    decimal Gil,
    DateTimeOffset CreatedDate);



public sealed record UpdateUserDto(
    [Required][EmailAddress] string Email,
    [Range(0, 1_000_000)] decimal Gil);

public static class Extensions
{
    public static UserDto AsDto(this ApplicationUser entity)
        => new(entity.Id, entity.UserName!, entity.Email!, entity.Gil, entity.CreatedOn);
}