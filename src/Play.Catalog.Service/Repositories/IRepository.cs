using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}