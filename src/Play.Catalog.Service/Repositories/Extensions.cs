using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        
        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var serviceSettings = configuration.GetRequiredSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;
            var mongoDbSettings = configuration.GetRequiredSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;
            
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }
    
    public static IServiceCollection AddMongoRepository<TEntity>(this IServiceCollection services, string collectionName)
        where TEntity : IEntity
    {
        services.AddSingleton<IRepository<TEntity>>(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return new MongoRepository<TEntity>(database, collectionName);
        });

        return services;
    }
}