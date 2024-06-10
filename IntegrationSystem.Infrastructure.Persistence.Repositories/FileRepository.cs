using System.Text.Json;
using System.Xml.Serialization;
using IntegrationSystem.Domain.Entities;
using IntegrationSystem.Infrastructure.Persistence.Interfaces;

namespace IntegrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// A repostory that reads and writes to a file.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to use.</typeparam>
/// <typeparam name="TKey">The type of the key of the entity to use.</typeparam>
/// <param name="filePath">The path of the file to read/write from/to</param>
public abstract class FileRepository<TEntity, TKey>(string filePath)
	: Repository<TEntity, TKey>, IFileRepository<TEntity, TKey>
	where TEntity : class
{
	public DateTime LastModified => File.GetLastWriteTimeUtc(filePath);

	/// <summary>
	/// Deserializes the data from the file to a type.
	/// </summary>
	/// <typeparam name="T">The type to convert <paramref name="data"/> to</typeparam>
	/// <param name="data">The data to convert</param>
	/// <returns>The converted <paramref name="data"/></returns>
	protected async Task<T?> DeserializeText<T>(string data)
	{
		// Check if the file is an XML file
		if (Path.GetExtension(filePath) == ".xml")
		{
			// Create a string reader to read the data
			using var stringReader = new StringReader(data);

			// Deserialize the data to the type
			var root = new XmlSerializer(typeof(T)).Deserialize(stringReader);

			// Return the root if it is not null, else return the default value of T
			return root is null ? default : (T)root;
		}
		else
			// Deserialize the data to the type
			return await Task.FromResult(JsonSerializer.Deserialize<T>(data));
	}

	/// <summary>
	/// Reads the file
	/// </summary>
	/// <returns>The contents of the file</returns>
	protected async Task<string> ReadFile() => await File.ReadAllTextAsync(filePath);
}
