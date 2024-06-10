using IntegrationSystem.Domain.Entities;

namespace IntegrationSystem.Application.Interfaces.UseCases;

/// <summary>
/// Interface for interacting with <see cref="MergedUser"/>.
/// </summary>
public interface IMergedUserUseCase
{
	/// <summary>
	/// Gets all <see cref="MergedUser"/> asynchronously.
	/// </summary>
	/// <returns>All <see cref="MergedUser"/></returns>
	Task<IEnumerable<MergedUser>> GetMergedUsersAsync();

	/// <summary>
	/// Deletes all <see cref="MergedUser"/> asynchronously.
	/// </summary>
	/// <returns>Awaitable Task</returns>
	Task DeleteMergedUsersAsync();

	/// <summary>
	/// Merges an <see cref="AzureADUser"/> with an <see cref="XmlUser"/> asynchronously.
	/// </summary>
	/// <param name="azureADUser">The <see cref="AzureADUser"/> to merge.</param>
	/// <param name="xmlUser">The <see cref="XmlUser"/> to merge.</param>
	/// <returns>Awaitable Task</returns>
	Task MergeUser(AzureADUser azureADUser, XmlUser xmlUser);

	/// <summary>
	/// Merges a collection of <see cref="AzureADUser"/> with a collection of <see cref="XmlUser"/> asynchronously.
	/// </summary>
	/// <param name="azureADUsers">The collection of <see cref="AzureADUser"/> to merge.</param>
	/// <param name="xmlUsers">The collection of <see cref="XmlUser"/> to merge.</param>
	/// <returns>Awaitable Task</returns>
	Task MergeUsers(IDictionary<string, AzureADUser> azureADUsers, IDictionary<string, XmlUser> xmlUsers);
}
