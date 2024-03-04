using System.Linq.Expressions;

namespace Play.Common;

public interface IRepository<TEntity>
    where TEntity : IEntity
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken
    );
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken
    );
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    Task RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken);
}
