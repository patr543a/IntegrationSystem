using IntegrationSystem.Domain.Entities;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegrationSystem.Infrastructure.Persistence.Repositories;

/// <summary>
/// A repository for the <see cref="MergedUser"/> entity.
/// </summary>
/// <param name="dbContext">The context to use to interact with the database</param>
public class MergedUserRepository(DbContext dbContext)
	: DbRepository<MergedUser, int>(dbContext), IMergedUserRepository
{
}
