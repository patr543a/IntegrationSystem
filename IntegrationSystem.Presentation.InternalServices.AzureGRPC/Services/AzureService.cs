using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IntegrationSystem.Domain.Entities;
using System.Net.Http.Headers;

namespace IntegrationSystem.Presentation.InternalServices.AzureGRPC.Services;

public class AzureService(ILogger<AzureService> logger, [FromKeyedServices("AzureADToken")] string azureADToken)
    : AzureAD.AzureADBase
{
    private readonly ILogger<AzureService> _logger = logger;
	private readonly string _azureADToken = azureADToken;

	public override Task<LastChangedReply> LastChanged(LastChangedRequest request, ServerCallContext context)
	{
		return Task.FromResult(new LastChangedReply
		{
			Time = Timestamp.FromDateTime(DateTime.UtcNow),
			Day = DateTime.UtcNow.Day,
			Month = DateTime.UtcNow.Month,
			Year = DateTime.UtcNow.Year
		});
	}

	public override async Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
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
		var reply = new GetUsersReply();

		reply.Users.AddRange(users.Select(user => new User
		{
			Id = user.UserId.ToString(),
			DisplayName = user.DisplayName,
			GivenName = user.FirstName,
			Surname = user.LastName,
			UserPrincipalName = user.UserPrincipalName,
			JobTitle = user.JobTitle,
			Mail = user.Email,
			MobilePhone = user.PhoneNumber,
			OfficeLocation = user.OfficeLocation,
			PreferredLanguage = user.PreferredLanguage,
		}));

		return reply;
	}
}
