using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public sealed class MongoRepository<TEntity>(IMongoDatabase database, string collectionName) 
    : IRepository<TEntity> where TEntity : IEntity
{
    private readonly IMongoCollection<TEntity> _dbCollection = database.GetCollection<TEntity>(collectionName);
    private readonly FilterDefinitionBuilder<TEntity> _filterBuilder = Builders<TEntity>.Filter;

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken) => 
        await _dbCollection.Find(_filterBuilder.Empty).ToListAsync(cancellationToken);

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(i => i.Id, id);
        return await _dbCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken) => 
        await _dbCollection.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(i => i.Id, entity.Id);
        await _dbCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions(), cancellationToken);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = _filterBuilder.Eq(i => i.Id, id);
        await _dbCollection.DeleteOneAsync(filter, new DeleteOptions(), cancellationToken);
    }
}