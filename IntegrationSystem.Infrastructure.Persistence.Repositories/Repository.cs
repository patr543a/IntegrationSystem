using IntegrationSystem.Infrastructure.Persistence.Interfaces;

namespace IntegrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// The base repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to use.</typeparam>
/// <typeparam name="TKey">The type of the key of the entity to use.</typeparam>
public abstract class Repository<TEntity, TKey>
	: IRepository<TEntity, TKey>
	where TEntity : class
{
	public abstract Task<IEnumerable<TEntity>> GetAllAsync();
}
