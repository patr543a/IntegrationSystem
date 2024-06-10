using IntegrationSystem.Domain.Entities;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// A repository for the <see cref="XmlUser"/> entity.
/// </summary>
/// <param name="xmlFileLocation">The path of the file to use</param>
public class XmlUserRepository([FromKeyedServices("XmlFileLocation")] string xmlFileLocation)
	: FileRepository<XmlUser, int>(xmlFileLocation), IXmlUserRepository
{
	public override async Task<IEnumerable<XmlUser>> GetAllAsync()
	{
		var xml = await ReadFile();

		return (await DeserializeText<XmlUsersRoot>(xml))?.Users ?? [];
	}
}