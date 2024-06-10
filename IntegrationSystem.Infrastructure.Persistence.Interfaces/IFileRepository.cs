namespace IntegrationSystem.Infrastructure.Persistence.Interfaces;

/// <summary>
/// Repository interface for file-based repositories.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to use.</typeparam>
/// <typeparam name="TKey">The type of the key of the entity to use.</typeparam>
public interface IFileRepository<TEntity, TKey>
	: IRepository<TEntity, TKey>
	where TEntity : class
{
	/// <summary>
	/// The last time the file was modified.
	/// </summary>
	DateTime LastModified { get; }
}
