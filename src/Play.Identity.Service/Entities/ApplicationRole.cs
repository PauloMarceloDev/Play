using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

namespace Play.Identity.Service.Entities;

[CollectionName("Roles")]
public sealed class ApplicationRole : MongoIdentityRole<Guid>
{
}
