using IntegrationSystem.Presentation.InternalServices.AzureGRPC.Services;
using Microsoft.Identity.Client;

DotNetEnv.Env.Load("./secrets.env");

// Get the environment variables
var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
var tenant_id = Environment.GetEnvironmentVariable("TENANT_ID");

// Create the confidential client
var confidentialApp = ConfidentialClientApplicationBuilder
	.Create(clientId)
	.WithClientSecret(clientSecret)
	.WithAuthority(new Uri($@"https://login.microsoftonline.com/{tenant_id}"))
	.Build();

// Get the token
var tokenResponse = await confidentialApp.AcquireTokenForClient([".default"])
	.ExecuteAsync();

// Make gRPC service
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddGrpc();
builder.Services.AddKeyedSingleton("AzureADToken", tokenResponse.AccessToken);

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.MapGrpcService<AzureService>();

app.Run();