using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;

namespace IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;

/// <summary>
/// The interface for a unit of work for the IntegrationSystem.
/// </summary>
public interface IIntegrationSystemUnitOfWork
	: IUnitOfWork
{
	/// <summary>
	/// The repository for the <see cref="MergedUser"/> entity.
	/// </summary>
	IMergedUserRepository MergedUserRepository { get; }

	/// <summary>
	/// The repository for the <see cref="XmlUser"/> entity.
	/// </summary>
	IXmlUserRepository XmlUserRepository { get; }
}
