using IntegrationSystem.Domain.Entities;

namespace IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;

/// <summary>
/// Repository interface for <see cref="XmlUser"/> entity.
/// </summary>
public interface IXmlUserRepository
	: IFileRepository<XmlUser, int>
{
}
