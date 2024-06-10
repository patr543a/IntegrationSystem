namespace IntegrationSystem.Infrastructure.Persistence.Interfaces;

/// <summary>
/// Repository interface to be implemented by all repositories.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to use.</typeparam>
/// <typeparam name="TKey">The type of the key of the entity to use.</typeparam>
public interface IRepository<TEntity, TKey>
	where TEntity : class
{
	/// <summary>
	/// Gets all entities.
	/// </summary>
	/// <returns>A collection of all entities</returns>
	Task<IEnumerable<TEntity>> GetAllAsync();
}
