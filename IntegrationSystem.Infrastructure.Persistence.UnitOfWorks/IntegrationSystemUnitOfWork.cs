using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace IntegrationSystem.Infrastructure.Persistence.UnitOfWorks;

/// <summary>
/// Unit of work for the IntegrationSystem.
/// </summary>
/// <param name="mergedUserRepository"><see cref="IMergedUserRepository"/> to use.</param>
/// <param name="xmlUserRepository"><see cref="IXmlUserRepository"/> to use.</param>
/// <param name="dbContext"></param>
public class IntegrationSystemUnitOfWork(
	IMergedUserRepository mergedUserRepository,
	IXmlUserRepository xmlUserRepository,
	DbContext dbContext)
	: UnitOfWork(dbContext), IIntegrationSystemUnitOfWork
{
	public IMergedUserRepository MergedUserRepository { get; } = mergedUserRepository;
	public IXmlUserRepository XmlUserRepository { get; } = xmlUserRepository;
}
