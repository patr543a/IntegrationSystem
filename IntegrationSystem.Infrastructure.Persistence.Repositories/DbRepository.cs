using IntegrationSystem.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// A repository for interacting with a database.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to use.</typeparam>
/// <typeparam name="TKey">The type of the key of the entity to use.</typeparam>
/// <param name="dbContext">The context to use to interact with the database</param>
public class DbRepository<TEntity, TKey>(DbContext dbContext)
	: Repository<TEntity, TKey>, IDbRepository<TEntity, TKey>
	where TEntity : class
{
	/// <summary>
	/// The database context.
	/// </summary>
	protected readonly DbContext _dbContext = dbContext;

	public async Task AddAsync(TEntity entity)
		=> await _dbContext.Set<TEntity>().AddAsync(entity);

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
		=> await _dbContext.Set<TEntity>().AddRangeAsync(entities);

	public async Task DeleteAllAsync()
		=> await _dbContext.Set<TEntity>().ExecuteDeleteAsync();

	public async Task UpdateAsync(TEntity entity)
		=> await Task.FromResult(_dbContext.Set<TEntity>().Update(entity));

	public async Task DeleteAsync(TEntity entity)
		=> await Task.FromResult(_dbContext.Set<TEntity>().Remove(entity));

	public override async Task<IEnumerable<TEntity>> GetAllAsync()
		=> await _dbContext.Set<TEntity>().ToArrayAsync();
}
