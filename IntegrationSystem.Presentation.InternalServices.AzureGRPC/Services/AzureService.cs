using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IntegrationSystem.Domain.Entities;
using System.Net.Http.Headers;

namespace IntegrationSystem.Presentation.InternalServices.AzureGRPC.Services;

/// <summary>
/// The gRPC service for Azure.
/// </summary>
/// <param name="logger">The logger to use.</param>
/// <param name="azureADToken">The bearer token to use.</param>
public class AzureService(ILogger<AzureService> logger, [FromKeyedServices("AzureADToken")] string azureADToken)
	: AzureAD.AzureADBase
{
	/// <summary>
	/// The logger.
	/// </summary>
	private readonly ILogger<AzureService> _logger = logger;

	/// <summary>
	/// The bearer token.
	/// </summary>
	private readonly string _azureADToken = azureADToken;

	/// <summary>
	/// The last time and value that the users were changed.
	/// </summary>
	private (DateTime Time, string Value) _lastChanged = (DateTime.MinValue, string.Empty);

	/// <summary>
	/// Gets the last changed date and time of the users.
	/// </summary>
	/// <param name="request">The request.</param>
	/// <param name="context">The request context.</param>
	/// <returns>The last changed date and time of the users.</returns>
	public override async Task<AzureLastChangedReply> AzureLastChanged(AzureLastChangedRequest request, ServerCallContext context)
	{
		// Get client
		using var client = new HttpClient();

		// Set the authorization header
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _azureADToken);

		// Get the users
		var response = await client.GetAsync("https://graph.microsoft.com/v1.0/users");
		var content = await response.Content.ReadAsStringAsync();

		// Check if the content has changed
		if (_lastChanged.Value != content)
			_lastChanged = (DateTime.UtcNow, content);

		// Return the last changed date and time
		return await Task.FromResult(new AzureLastChangedReply
		{
			Time = Timestamp.FromDateTime(_lastChanged.Time),
			Day = _lastChanged.Time.Day,
			Month = _lastChanged.Time.Month,
			Year = _lastChanged.Time.Year
		});
	}

	/// <summary>
	/// Gets the users.
	/// </summary>
	/// <param name="request">The request.</param>
	/// <param name="context">The request context.</param>
	/// <returns>The users.</returns>
	public override async Task<AzureGetUsersReply> AzureGetUsers(AzureGetUsersRequest request, ServerCallContext context)
	{
		// Get client
		using var client = new HttpClient();

		// Set the authorization header
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _azureADToken);

		// Get the users
		var response = await client.GetAsync("https://graph.microsoft.com/v1.0/users");
		var content = await response.Content.ReadFromJsonAsync<AzureADRoot>();
		var users = content?.Users ?? [];

		// Create the reply
		var reply = new AzureGetUsersReply();

		// Add the users
		reply.Users.AddRange(users.Select(user => new AzureUser
		{
			Id = user.UserId.ToString(),
			DisplayName = user.DisplayName,
			GivenName = user.FirstName,
			Surname = user.LastName,
			UserPrincipalName = user.UserPrincipalName,
			JobTitle = user.JobTitle ?? string.Empty,
			Mail = user.Email ?? string.Empty,
			MobilePhone = user.PhoneNumber ?? string.Empty,
			OfficeLocation = user.OfficeLocation ?? string.Empty,
			PreferredLanguage = user.PreferredLanguage ?? string.Empty,
		}));

		return reply;
	}
}
