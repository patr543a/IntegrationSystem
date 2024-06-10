using Grpc.Net.Client;
using IntegrationSystem.Application.Interfaces.UseCases;
using IntegrationSystem.Domain.Entities;

namespace IntegrationSystem.Presentation.InternalServices.UserMerger.Services;

/// <summary>
/// Background service that merges users from Azure and XML services.
/// </summary>
/// <param name="serviceScopeFactory">The service scope factory.</param>
/// <param name="azureServerAddress">The address of the Azure gRPC server.</param>
/// <param name="xmlServerAddress">The address of the Xml gRPC server.</param>
public class BackgroundUserMergerService(
	IServiceScopeFactory serviceScopeFactory,
	[FromKeyedServices("Azure_gRPC_Address")] string azureServerAddress,
	[FromKeyedServices("XML_gRPC_Address")] string xmlServerAddress)
	: BackgroundService
{
	/// <summary>
	/// The service scope factory.
	/// </summary>
	private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

	/// <summary>
	/// The address of the Azure gRPC server.
	/// </summary>
	private readonly string _azureServerAddress = azureServerAddress;

	/// <summary>
	/// The address of the Xml gRPC server.
	/// </summary>
	private readonly string _xmlServerAddress = xmlServerAddress;

	/// <summary>
	/// The last time the Azure service was changed.
	/// </summary>
	private DateTime _lastChangedAzure = DateTime.MinValue;

	/// <summary>
	/// The last time the Xml service was changed.
	/// </summary>
	private DateTime _lastChangedXml = DateTime.MinValue;

	/// <summary>
	/// Executes the background service.
	/// </summary>
	/// <returns>Awaitable Task</returns>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// Loop until the service is stopped
		while (!stoppingToken.IsCancellationRequested)
		{
			// Create gRPC channels for the Azure and Xml services
			var azureChannel = GrpcChannel.ForAddress(_azureServerAddress);
			var xmlChannel = GrpcChannel.ForAddress(_xmlServerAddress);
			var azureClient = new AzureAD.AzureADClient(azureChannel);
			var xmlClient = new XML.XMLClient(xmlChannel);

			// Get the last time the Azure and Xml services were changed
			var azureLastChanged = await azureClient.AzureLastChangedAsync(new AzureLastChangedRequest(), cancellationToken: stoppingToken);
			var xmlLastChanged = await xmlClient.XMLLastChangedAsync(new XMLLastChangedRequest(), cancellationToken: stoppingToken);
			var azureLastChangedDateTime = azureLastChanged.Time.ToDateTime().AddYears(azureLastChanged.Year).AddMonths(azureLastChanged.Month).AddDays(azureLastChanged.Day);
			var xmlLastChangedDateTime = xmlLastChanged.Time.ToDateTime().AddYears(xmlLastChanged.Year).AddMonths(xmlLastChanged.Month).AddDays(xmlLastChanged.Day);

			// Check if the Azure or Xml services have changed
			var changed = false;

			if (azureLastChangedDateTime > _lastChangedAzure)
				(_lastChangedAzure, changed) = (azureLastChangedDateTime, true);

			if (xmlLastChangedDateTime > _lastChangedXml)
				(_lastChangedXml, changed) = (xmlLastChangedDateTime, true);

			// If the services have not changed, wait for an hour before checking again
			if (!changed)
			{
				await Task.Delay(3600_000, stoppingToken);

				return;
			}

			// Get the users from the Azure and Xml services
			var azureUsersReply = await azureClient.AzureGetUsersAsync(new AzureGetUsersRequest(), cancellationToken: stoppingToken);
			var xmlUsersReply = await xmlClient.XMLGetUsersAsync(new XMLGetUsersRequest(), cancellationToken: stoppingToken);

			// Create dictionaries of the users
			var azureUsers = azureUsersReply.Users.Select(u => new AzureADUser
			{
				UserId = new Guid(u.Id),
				DisplayName = u.DisplayName,
				FirstName = u.GivenName,
				LastName = u.Surname,
				UserPrincipalName = u.UserPrincipalName,
				JobTitle = u.JobTitle,
				Email = u.Mail,
				PhoneNumber = u.MobilePhone,
				OfficeLocation = u.OfficeLocation,
				PreferredLanguage = u.PreferredLanguage,
			}).ToDictionary(u =>
			{
				var name = u.DisplayName.Split();

				return $"{name[0]} {name[^1]}";
			});

			// Create dictionaries of the users
			var xmlUsers = xmlUsersReply.Users.Select(u => new XmlUser
			{
				Number = u.Number,
				Name = u.Name,
				Title = u.Title,
				Address = u.Address,
				City = u.City,
				Email = u.Email,
				Mobile = u.Mobile,
			}).ToDictionary(u =>
			{
				var name = u.Name.Split();

				return $"{name[0]} {name[^1]}";
			});

			// Get the merged user use case
			using var scope = _serviceScopeFactory.CreateScope();

			var mergedUserUseCase = scope.ServiceProvider.GetRequiredService<IMergedUserUseCase>();

			// Merge the users
			await mergedUserUseCase.MergeUsers(azureUsers, xmlUsers);

			// Wait for a second before checking again
			await Task.Delay(1000, stoppingToken);
		}
	}
}
