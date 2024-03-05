using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

namespace Play.Identity.Service.Entities;

[CollectionName("Users")]
public sealed class ApplicationUser : MongoIdentityUser<Guid>
{
    public decimal Gil { get; set; }
}
