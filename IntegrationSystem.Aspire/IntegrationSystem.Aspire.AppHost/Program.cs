// Make Aspire service
var builder = DistributedApplication.CreateBuilder(args);

// Add projects
builder.AddProject<Projects.IntegrationSystem_Presentation_APIs_IntegrationAPI>("IntegrationAPI");

var azureGRPC = builder.AddProject<Projects.IntegrationSystem_Presentation_InternalServices_AzureGRPC>("AzureAD-gRPC");

var xmlGRPC = builder.AddProject<Projects.IntegrationSystem_Presentation_InternalServices_XML_GRPC>("XML-gRPC");

builder.AddProject<Projects.IntegrationSystem_Presentation_InternalServices_UserMerger>("UserMerger")
	.WithReference(azureGRPC)
	.WithReference(xmlGRPC);

// Build and run
builder.Build().Run();