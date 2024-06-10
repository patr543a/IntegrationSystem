using System.Xml.Serialization;

namespace IntegrationSystem.Domain.Entities;

/// <summary>
/// Root of the XML file.
/// </summary>
[XmlRoot("data")]
public class XmlUsersRoot
{
	/// <summary>
	/// Object with the users.
	/// </summary>
	[XmlArray("persons")]
	[XmlArrayItem("person")]
	public required List<XmlUser> Users { get; set; }
}

/// <summary>
/// User of the XML file.
/// </summary>
public class XmlUser
{
	/// <summary>
	/// The number of the user.
	/// </summary>
	[XmlAttribute("number")]
	public int Number { get; set; }

	/// <summary>
	/// The name of the user.
	/// </summary>
	[XmlElement("name")]
	public required string Name { get; set; }

	/// <summary>
	/// The title of the user.
	/// </summary>
	[XmlElement("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// The address of the user.
	/// </summary>
	[XmlElement("address")]
	public required string Address { get; set; }

	/// <summary>
	/// The city of the user.
	/// </summary>
	[XmlElement("city")]
	public required string City { get; set; }

	/// <summary>
	/// The email address of the user.
	/// </summary>
	[XmlElement("email")]
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// The mobile number of the user.
	/// </summary>
	[XmlElement("mobile")]
	public string Mobile { get; set; } = string.Empty;
}
