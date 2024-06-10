using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace IntegrationSystem.Domain.Entities;

/// <summary>
/// Represents a merged user of <see cref="AzureADUser"/> and <see cref="XmlUser"/>.
/// </summary>
public class MergedUser
{
	/// <summary>
	/// The unique id of the merged user.
	/// </summary>
	public int MergedUserId { get; set; }

	/// <summary>
	/// The unique azure id of the user.
	/// </summary>
	[JsonPropertyName("AzureId")]
	public Guid? AzureUserId { get; set; }

	/// <summary>
	/// The unique xml id of the user.
	/// </summary>
	[XmlAttribute("xmlId")]
	public int? XmlId { get; set; }

	/// <summary>
	/// The full name of the user.
	/// </summary>
	[JsonPropertyName("fullName")]
	public string? FullName { get; set; }

	/// <summary>
	/// The user principal name (UPN) of the user.
	/// </summary>
	[JsonPropertyName("userPrincipalName")]
	public string? UserPrincipalName { get; set; }

	/// <summary>
	/// The azure job title of the user.
	/// </summary>
	[JsonPropertyName("azureJobTitle")]
	public string? AzureJobTitle { get; set; }

	/// <summary>
	/// The title of the user.
	/// </summary>
	[XmlElement("xmlJobTitle")]
	public string? XmlJobTitle { get; set; } = string.Empty;

	/// <summary>
	/// The azure email address of the user.
	/// </summary>
	[JsonPropertyName("azureEmail")]
	public string? AzureEmail { get; set; }

	/// <summary>
	/// The email address of the user.
	/// </summary>
	[XmlElement("xmlEmail")]
	public string? XmlEmail { get; set; } = string.Empty;

	/// <summary>
	/// The azure phone number of the user.
	/// </summary>
	[JsonPropertyName("azurePhoneNumber")]
	public string? AzurePhoneNumber { get; set; }

	/// <summary>
	/// The xml phone number of the user.
	/// </summary>
	[XmlElement("XmlPhoneNumber")]
	public string? XmlPhoneNumber { get; set; } = string.Empty;

	/// <summary>
	/// The azure office location of the user.
	/// </summary>
	[JsonPropertyName("officeLocation")]
	public string? OfficeLocation { get; set; }

	/// <summary>
	/// The preferred language of the user.
	/// </summary>
	[JsonPropertyName("preferredLanguage")]
	public string? PreferredLanguage { get; set; }	

	/// <summary>
	/// The address of the user.
	/// </summary>
	[XmlElement("address")]
	public string? Address { get; set; }

	/// <summary>
	/// The city of the user.
	/// </summary>
	[XmlElement("city")]
	public string? City { get; set; }
}
