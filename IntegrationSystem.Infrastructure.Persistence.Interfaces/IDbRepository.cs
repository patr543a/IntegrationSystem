namespace IntegrationSystem.Infrastructure.Persistence.Interfaces;

/// <summary>
/// Repository interface for database operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to use.</typeparam>
/// <typeparam name="TKey">The type of the key of the entity to use.</typeparam>
public interface IDbRepository<TEntity, TKey>
	: IRepository<TEntity, TKey>
	where TEntity : class
{
	/// <summary>
	/// Adds an entity to the database.
	/// </summary>
	/// <param name="entity">The entity to add</param>
	/// <returns>Awaitable Task</returns>
	Task AddAsync(TEntity entity);

	/// <summary>
	/// Adds a range of entities to the database.
	/// </summary>
	/// <param name="entities">The entities to add</param>
	/// <returns>Awaitable Task</returns>
	Task AddRangeAsync(IEnumerable<TEntity> entities);

	/// <summary>
	/// Updates an entity in the database.
	/// </summary>
	/// <param name="entity">The entity to update</param>
	/// <returns>Awaitable Task</returns>
	Task UpdateAsync(TEntity entity);

	/// <summary>
	/// Deletes an entity from the database.
	/// </summary>
	/// <param name="entity">The entity to delete</param>
	/// <returns>Awaitable Task</returns>
	Task DeleteAsync(TEntity entity);

	/// <summary>
	/// Deletes all entities from the database.
	/// </summary>
	/// <returns>Awaitable Task</returns>
	Task DeleteAllAsync();
}
