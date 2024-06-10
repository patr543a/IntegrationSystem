using IntegrationSystem.Domain.Entities;

namespace IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;

/// <summary>
/// Repository interface for <see cref="MergedUser"/> entity.
/// </summary>
public interface IMergedUserRepository
	: IDbRepository<MergedUser, int>
{
}
