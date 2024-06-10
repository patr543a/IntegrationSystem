using IntegrationSystem.Application.Interfaces.UseCases;
using IntegrationSystem.Application.UseCases;
using IntegrationSystem.Infrastructure.Persistence.Contexts;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.Repositories;
using IntegrationSystem.Infrastructure.Persistence.Interfaces.UnitOfWorks;
using IntegrationSystem.Infrastructure.Persistence.Repositories;
using IntegrationSystem.Infrastructure.Persistence.UnitOfWorks;
using IntegrationSystem.Presentation.InternalServices.UserMerger.Services;
using Microsoft.EntityFrameworkCore;

// Make UserMerger service
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.AddServiceDefaults();

builder.Services.AddDbContext<DbContext, MergeUserContext>();
builder.Services.AddTransient<IMergedUserRepository, MergedUserRepository>();
builder.Services.AddTransient<IXmlUserRepository, XmlUserRepository>();
builder.Services.AddTransient<IIntegrationSystemUnitOfWork, IntegrationSystemUnitOfWork>();
builder.Services.AddTransient<IMergedUserUseCase, MergedUserUseCase>();
builder.Services.AddHostedService<BackgroundUserMergerService>();
builder.Services.AddKeyedSingleton("XmlFileLocation", "./TestData.xml");
builder.Services.AddKeyedSingleton("Azure_gRPC_Address", "http://localhost:5041");
builder.Services.AddKeyedSingleton("XML_gRPC_Address", "http://localhost:5209");

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapDefaultEndpoints();

app.Run();
