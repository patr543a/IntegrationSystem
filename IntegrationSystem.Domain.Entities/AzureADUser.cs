using System.Text.Json.Serialization;

namespace IntegrationSystem.Domain.Entities;

/// <summary>
/// Represents the root object of an AzureAD response.
/// </summary>
public class AzureADRoot
{
	/// <summary>
	/// The collection of <see cref="AzureADUser"/> users.
	/// </summary>
	[JsonPropertyName("value")]
	public required IEnumerable<AzureADUser> Users { get; set; }
}

/// <summary>
/// Represents a user from AzureAD.
/// </summary>
public class AzureADUser
{
	/// <summary>
	/// The unique identifier of the user.
	/// </summary>
	[JsonPropertyName("id")]
    public required Guid UserId { get; set; }

	/// <summary>
	/// The display name of the user.
	/// </summary>
	[JsonPropertyName("displayName")]
	public required string DisplayName { get; set; }

	/// <summary>
	/// The first name of the user. May also include middle name.
	/// </summary>
	[JsonPropertyName("givenName")]
    public required string FirstName { get; set; }

	/// <summary>
	/// The last name of the user. May also include middle name.
	/// </summary>
	[JsonPropertyName("surname")]
    public required string LastName { get; set; }

	/// <summary>
	/// The user principal name (UPN) of the user.
	/// </summary>
	[JsonPropertyName("userPrincipalName")]
    public required string UserPrincipalName { get; set; }

	/// <summary>
	/// The job title of the user.
	/// </summary>
	[JsonPropertyName("jobTitle")]
    public string? JobTitle { get; set; }

	/// <summary>
	/// The email address of the user.
	/// </summary>
	[JsonPropertyName("mail")]
    public string? Email { get; set; }

	/// <summary>
	/// The mobile phone number of the user.
	/// </summary>
	[JsonPropertyName("mobilePhone")]
    public string? PhoneNumber { get; set; }

	/// <summary>
	/// The office location of the user.
	/// </summary>
	[JsonPropertyName("officeLocation")]
    public string? OfficeLocation { get; set; }

	/// <summary>
	/// The preferred language of the user.
	/// </summary>
	[JsonPropertyName("preferredLanguage")]
    public string? PreferredLanguage { get; set; }
}
