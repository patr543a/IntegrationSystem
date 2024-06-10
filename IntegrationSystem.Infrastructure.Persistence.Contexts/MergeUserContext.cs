using IntegrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationSystem.Infrastructure.Persistence.Contexts;

/// <summary>
/// Database context for IntegrationSystem.
/// </summary>
public class MergeUserContext
	: DbContext
{
	/// <summary>
	/// Set of <c>MergedUser</c>.
	/// </summary>
	public DbSet<MergedUser> MergedUsers { get; set; }

	/// <summary>
	/// Sets up the database connection.
	/// </summary>
	/// <param name="optionsBuilder">The <c>DbContextOptionsBuilder</c> to use.</param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IntegrationSystemDB;Trusted_Connection=true;");

	/// <summary>
	/// Sets up the database model.
	/// </summary>
	/// <param name="modelBuilder">The <c>ModelBuilder</c> to use.</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Configure the MergedUser entity
		modelBuilder.Entity<MergedUser>(entity =>
		{
			// Set the primary key
			entity.HasKey(mu => mu.MergedUserId);

			// Set the properties
			entity.Property(mu => mu.AzureUserId);

			entity.Property(mu => mu.XmlId);

			entity.Property(mu => mu.FullName)
				.HasMaxLength(100);

			entity.Property(mu => mu.UserPrincipalName)
				.HasMaxLength(100);

			entity.Property(mu => mu.AzureJobTitle)
				.HasMaxLength(100);

			entity.Property(mu => mu.XmlJobTitle)
				.HasMaxLength(100);

			entity.Property(mu => mu.AzureEmail)
				.HasMaxLength(100);

			entity.Property(mu => mu.XmlEmail)
				.HasMaxLength(100);

			entity.Property(mu => mu.AzurePhoneNumber)
				.HasMaxLength(100);

			entity.Property(mu => mu.XmlPhoneNumber)
				.HasMaxLength(100);

			entity.Property(mu => mu.OfficeLocation)
				.HasMaxLength(100);

			entity.Property(mu => mu.PreferredLanguage)
				.HasMaxLength(100);

			entity.Property(mu => mu.Address)
				.HasMaxLength(100);

			entity.Property(mu => mu.City)
				.HasMaxLength(100);
		});
	}
}
