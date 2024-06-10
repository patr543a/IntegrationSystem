namespace IntegrationSystem.Infrastructure.Persistence.Interfaces;

/// <summary>
/// Unit of work interface to be implemented by all unit of works.
/// </summary>
public interface IUnitOfWork
{
	/// <summary>
	/// Commits all changes made.
	/// </summary>
	/// <returns>The number of entities/rows affected</returns>
	Task<int> CommitAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Rollback all changes made.
	/// </summary>
	/// <returns>Awaitable Task</returns>
	Task RollbackAsync(CancellationToken cancellationToken = default);
}
