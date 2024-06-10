using IntegrationSystem.Infrastructure.Persistence.Contexts;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;
using IntegrationSystem.Infrastructure.Persistence.Repositories;
using IntegrationSystem.Infrastructure.Persistence.UnitOfWorks;
using IntegrationSystem.Presentation.InternalServices.XML_GRPC.Services;
using Microsoft.EntityFrameworkCore;

// Make gRPC service
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddServiceDefaults();

builder.Services.AddGrpc();

builder.Services.AddDbContext<DbContext, MergeUserContext>();
builder.Services.AddTransient<IMergedUserRepository, MergedUserRepository>();
builder.Services.AddKeyedSingleton("XmlFileLocation", "./TestData.xml");
builder.Services.AddTransient<IXmlUserRepository, XmlUserRepository>();
builder.Services.AddTransient<IIntegrationSystemUnitOfWork, IntegrationSystemUnitOfWork>();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapDefaultEndpoints();

app.MapGrpcService<XmlService>();

app.Run();
