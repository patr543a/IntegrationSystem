using IntegrationSystem.Application.Interfaces.UseCases;
using IntegrationSystem.Domain.Entities;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;

namespace IntegrationSystem.Application.UseCases;

/// <summary>
/// A use case for managing <see cref="MergedUser"/>.
/// </summary>
/// <param name="unitOfWork"></param>
public class MergedUserUseCase(IIntegrationSystemUnitOfWork unitOfWork)
	: IMergedUserUseCase
{
	/// <summary>
	/// The unit of work.
	/// </summary>
	private readonly IIntegrationSystemUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<MergedUser>> GetMergedUsersAsync()
		=> await _unitOfWork
			.MergedUserRepository
			.GetAllAsync();

	public async Task DeleteMergedUsersAsync()
		=> await _unitOfWork
			.MergedUserRepository
			.DeleteAllAsync();

	public async Task MergeUser(AzureADUser azureADUser, XmlUser xmlUser)
	{
		// Merge user
		var mergedUser = MergeUserInternal(azureADUser, xmlUser);

		// Add merged user to the database
		await _unitOfWork.MergedUserRepository.AddAsync(mergedUser);

		// Commit changes
		await _unitOfWork.CommitAsync();
	}

	public async Task MergeUsers(IDictionary<string, AzureADUser> azureADUsers, IDictionary<string, XmlUser> xmlUsers)
	{
		// Get all distinct keys
		var keys = azureADUsers.Keys
			.Concat(xmlUsers.Keys)
			.Distinct();

		// Create a list of merged users of the same size as the number of distinct keys
		var mergedUsers = new List<MergedUser>(keys.Count());

		foreach (var name in keys)
		{
			// Get the AzureAD user and the XML user
			azureADUsers.TryGetValue(name, out var azureADUser);
			xmlUsers.TryGetValue(name, out var xmlUser);

			// Create a merged user
			if (azureADUser is null)
				// Create a merged user with the XML user
				mergedUsers.Add(new MergedUser
				{
					XmlId = ConvertToNullIfEmpty(xmlUser?.Number),
					FullName = ConvertToNullIfEmpty(xmlUser?.Name),
					XmlJobTitle = ConvertToNullIfEmpty(xmlUser?.Title),
					XmlEmail = ConvertToNullIfEmpty(xmlUser?.Email),
					XmlPhoneNumber = ConvertToNullIfEmpty(xmlUser?.Mobile),
					Address = ConvertToNullIfEmpty(xmlUser?.Address),
					City = ConvertToNullIfEmpty(xmlUser?.City),

					AzureUserId = null,
					UserPrincipalName = null,
					AzureJobTitle = null,
					AzureEmail = null,
					AzurePhoneNumber = null,
					OfficeLocation = null,
					PreferredLanguage = null,
				});
			else if (xmlUser is null)
				// Create a merged user with the AzureAD user
				mergedUsers.Add(new MergedUser
				{
					AzureUserId = ConvertToNullIfEmpty(azureADUser.UserId),
					FullName = ConvertToNullIfEmpty(azureADUser.DisplayName),
					UserPrincipalName = ConvertToNullIfEmpty(azureADUser.UserPrincipalName),
					AzureJobTitle = ConvertToNullIfEmpty(azureADUser.JobTitle),
					AzureEmail = ConvertToNullIfEmpty(azureADUser.Email),
					AzurePhoneNumber = ConvertToNullIfEmpty(azureADUser.PhoneNumber),
					OfficeLocation = ConvertToNullIfEmpty(azureADUser.OfficeLocation),
					PreferredLanguage = ConvertToNullIfEmpty(azureADUser.PreferredLanguage),

					XmlId = null,
					XmlJobTitle = null,
					XmlEmail = null,
					XmlPhoneNumber = null,
					Address = null,
					City = null,
				});
			else
				// Merge the AzureAD user with the XML user
				mergedUsers.Add(MergeUserInternal(azureADUser, xmlUser));
		}

		// Get all existing users
		var existingUsers = await _unitOfWork.MergedUserRepository.GetAllAsync();

		// Get new, updated, and deleted users
		var newUsers = mergedUsers.Where(mu => !existingUsers.Any(eu => eu.FullName == mu.FullName));
		var updatedUsers = mergedUsers.Where(mu => existingUsers.Any(eu => eu.FullName == mu.FullName));
		var deletedUsers = existingUsers.Where(eu => !mergedUsers.Any(mu => mu.FullName == eu.FullName));

		// Add new users
		await _unitOfWork.MergedUserRepository.AddRangeAsync(newUsers);

		// Update existing users
		foreach (var updatedUser in updatedUsers)
		{
			// Get the existing user
			var existingUser = existingUsers.First(eu => eu.FullName == updatedUser.FullName);

			// Update the existing user
			existingUser.AzureUserId = ConvertToNullIfEmpty(updatedUser.AzureUserId);
			existingUser.XmlId = ConvertToNullIfEmpty(updatedUser.XmlId);
			existingUser.FullName = ConvertToNullIfEmpty(updatedUser.FullName);
			existingUser.UserPrincipalName = ConvertToNullIfEmpty(updatedUser.UserPrincipalName);
			existingUser.AzureJobTitle = ConvertToNullIfEmpty(updatedUser.AzureJobTitle);
			existingUser.XmlJobTitle = ConvertToNullIfEmpty(updatedUser.XmlJobTitle);
			existingUser.AzureEmail = ConvertToNullIfEmpty(updatedUser.AzureEmail);
			existingUser.XmlEmail = ConvertToNullIfEmpty(updatedUser.XmlEmail);
			existingUser.AzurePhoneNumber = ConvertToNullIfEmpty(updatedUser.AzurePhoneNumber);
			existingUser.XmlPhoneNumber = ConvertToNullIfEmpty(updatedUser.XmlPhoneNumber);
			existingUser.OfficeLocation = ConvertToNullIfEmpty(updatedUser.OfficeLocation);
			existingUser.PreferredLanguage = ConvertToNullIfEmpty(updatedUser.PreferredLanguage);
			existingUser.Address = ConvertToNullIfEmpty(updatedUser.Address);
			existingUser.City = ConvertToNullIfEmpty(updatedUser.City);

			// Update the existing user in the database
			await _unitOfWork.MergedUserRepository.UpdateAsync(existingUser);
		}

		// Delete users
		foreach (var deletedUser in deletedUsers)
			await _unitOfWork.MergedUserRepository.DeleteAsync(deletedUser);

		// Commit changes
		await _unitOfWork.CommitAsync();
	}

	/// <summary>
	/// Merges an <see cref="AzureADUser"/> user with an <see cref="XmlUser"/> user.
	/// </summary>
	/// <param name="azureADUser">The <see cref="AzureADUser"/> to merge.</param>
	/// <param name="xmlUser">The <see cref="XmlUser"/> to merge.</param>
	/// <returns></returns>
	private static MergedUser MergeUserInternal(AzureADUser azureADUser, XmlUser xmlUser)
		=> new()
		{
			AzureUserId = ConvertToNullIfEmpty(azureADUser.UserId),
			XmlId = ConvertToNullIfEmpty<int>(xmlUser.Number),
			FullName = ConvertToNullIfEmpty(xmlUser.Name),
			UserPrincipalName = ConvertToNullIfEmpty(azureADUser.UserPrincipalName),
			AzureJobTitle = ConvertToNullIfEmpty(azureADUser.JobTitle),
			XmlJobTitle = ConvertToNullIfEmpty(xmlUser.Title),
			AzureEmail = ConvertToNullIfEmpty(azureADUser.Email),
			XmlEmail = ConvertToNullIfEmpty(xmlUser.Email),
			AzurePhoneNumber = ConvertToNullIfEmpty(azureADUser.PhoneNumber),
			XmlPhoneNumber = ConvertToNullIfEmpty(xmlUser.Mobile),
			OfficeLocation = ConvertToNullIfEmpty(azureADUser.OfficeLocation),
			PreferredLanguage = ConvertToNullIfEmpty(azureADUser.PreferredLanguage),
			Address = ConvertToNullIfEmpty(xmlUser.Address),
			City = ConvertToNullIfEmpty(xmlUser.City),
		};

	/// <summary>
	/// Converts a string to <see langword="null"/> if it is <see langword="null"/> or empty.
	/// </summary>
	/// <param name="value">The value to convert</param>
	/// <returns><see langword="null"/> if null or empty otherwise string.</returns>
	private static string? ConvertToNullIfEmpty(string? value)
		=> string.IsNullOrWhiteSpace(value) ? null : value;

	/// <summary>
	/// Converts a <see cref="Guid"/> to <see langword="null"/> if it is <see cref="Guid.Empty"/> or <see langword="null"/>.
	/// </summary>
	/// <param name="value">The value to convert</param>
	/// <returns><see langword="null"/> if null or empty otherwise Guid.</returns>
	private static Guid? ConvertToNullIfEmpty(Guid? value)
		=> value == Guid.Empty || value is null ? null : value;

	/// <summary>
	/// Converts a <see cref="T"/> to <see langword="null"/> if it is <see cref="default"/> or <see langword="null"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	private static T? ConvertToNullIfEmpty<T>(T? value)
		where T : struct
		=> value.Equals(default) || value is null ? null : value;
}
