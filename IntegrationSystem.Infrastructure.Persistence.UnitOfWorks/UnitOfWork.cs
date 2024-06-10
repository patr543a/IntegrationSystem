using IntegrationSystem.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationSystem.Infrastructure.Persistence.UnitOfWorks;

/// <summary>
/// The base unit of work.
/// </summary>
/// <param name="dbContext">The context to use to interact with the database</param>
public class UnitOfWork(DbContext dbContext)
	: IUnitOfWork
{
	/// <summary>
	/// The database context.
	/// </summary>
	private readonly DbContext _dbContext = dbContext;

	public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		=> await _dbContext.SaveChangesAsync(cancellationToken);

	public Task RollbackAsync(CancellationToken cancellationToken = default)
		=> throw new InvalidOperationException("Changes was cancelled");
}
